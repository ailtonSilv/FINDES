using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FINDES.Plugin;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;
using FINDES.Plugin.CRMFields;
using FINDES.ConsoleCarga.Util;

namespace FINDES.ConsoleCarga
{
    public class Load
    {
        public static CrmServiceClient conn;
        public static IOrganizationService organizationService;
        public static string ax4bConnectorWebApiURL;

        /// <summary>
        /// Retrieves the current users timezone code
        /// </summary>
        /// <param name="service"> IOrganizationService </param>
        /// <returns></returns>
        private static int? RetrieveCurrentUsersSettings(IOrganizationService service)
        {
            var currentUserSettings = service.RetrieveMultiple(
                new QueryExpression("usersettings")
                {
                    ColumnSet = new ColumnSet("timezonecode"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                        new ConditionExpression("systemuserid", ConditionOperator.EqualUserId)
                        }
                    }
                }).Entities[0].ToEntity<Entity>();
            //return time zone code
            return (int?)currentUserSettings.Attributes["timezonecode"];
        }

        public static void DateToLocalTime()
        {
            ax4bConnectorWebApiURL = ConfigurationManager.AppSettings.Get("AX4BConnectorWebApi").ToString();

            CheckCRMConnection();

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            ConsoleHelper.Log("TimeZoneCode" + timeZone);

            DateTime dataModificacao = new DateTime(2020, 2, 1);

            var query = new QueryExpression("account");
            query.ColumnSet = new ColumnSet("accountid", "findes_cnpj", "createdon", "modifiedon");
            query.Criteria.AddFilter(LogicalOperator.And);
            query.Criteria.Filters[0].AddCondition("modifiedon", ConditionOperator.GreaterThan, dataModificacao);
            query.TopCount = 10;
            query.Orders.Add(new OrderExpression("createdon", OrderType.Ascending));
            EntityCollection etc = organizationService.RetrieveMultiple(query);

            foreach (Entity item in etc.Entities)
            {
                Guid id = item.GetAttributeValue<Guid>("accountid");
                string cnpj = item.GetAttributeValue<string>("findes_cnpj");
                DateTime created = TimeZoneInfo.ConvertTime(item.GetAttributeValue<DateTime>("createdon"), timeZone);
                DateTime modified = TimeZoneInfo.ConvertTime(item.GetAttributeValue<DateTime>("modifiedon"), timeZone);
                var verao = timeZone.IsDaylightSavingTime(item.GetAttributeValue<DateTime>("modifiedon"));
                //if (verao) { modified = modified.AddHours(1); }
                ConsoleHelper.Log($"{id.ToString()} / {cnpj} / {modified.ToString("yyyy/MM/dd HH:mm:ss")} {verao.ToString()} / { created.ToString("yyyy/MM/dd HH:mm:ss")}");
            }

            Console.ReadLine();
        }

        public static void Execute()
        {
            try
            {
                #region | AppSettings
                ax4bConnectorWebApiURL = ConfigurationManager.AppSettings.Get("AX4BConnectorWebApi").ToString();
                bool filterByCargaInicial = bool.Parse(ConfigurationManager.AppSettings.Get("FilterbyCargaInicial").ToString());
                string method = ConfigurationManager.AppSettings.Get("Method").ToString();
                string entities = ConfigurationManager.AppSettings.Get("Entities").ToString();
                List<string> listEntities = entities.Split(';').ToList<string>();
                bool testMode = bool.Parse(ConfigurationManager.AppSettings.Get("TestMode").ToString());
                #endregion

                #region | Connect to CRM
                CheckCRMConnection();
                #endregion

                #region | Execute
                foreach (string str in listEntities)
                {
                    switch (str)
                    {
                        case "account":
                            Account.Send(method, filterByCargaInicial, testMode);
                            break;
                        case "atividadeeconomica":
                            AtividadeEconomica.Send(method, filterByCargaInicial, testMode);
                            break;
                        case "contact":
                            Contact.Send(method, filterByCargaInicial, testMode);
                            break;
                        case "estado":
                            Estado.Send(method, filterByCargaInicial, testMode); ;
                            break;
                        case "municipio":
                            Municipio.Send(method, filterByCargaInicial, testMode);
                            break;
                        case "product":
                            Product.Send(method, filterByCargaInicial, testMode);
                            break;
                        case "regional":
                            Regional.Send(method, filterByCargaInicial, testMode);
                            break;
                        case "salesorder":
                            Product.Send(method, filterByCargaInicial, testMode);
                            break;
                        case "salesorderdetail":
                            Product.Send(method, filterByCargaInicial, testMode);
                            break;
                        case "sindicato":
                            Sindicato.Send(method, filterByCargaInicial, testMode);
                            break;
                        case "unidadedeatendimento":
                            UnidadedeAtendimento.Send(method, filterByCargaInicial, testMode);
                            break;

                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                ConsoleHelper.Log(ex.Message);
                ConsoleHelper.Log(ex.StackTrace);
            }
            
        }

        public static void CheckCRMConnection()
        {
            if (conn == null || !conn.IsReady)
            {
                ConnectCRM();
            }
        }

        public static void ConnectCRM()
        {
            ConsoleHelper.Log("CRM > Authentication > Started");
            string connStr = ConfigurationManager.ConnectionStrings["FINDES"].ConnectionString;
            if (connStr.ToLower().IndexOf("password") > 0)
            {
                var pwdList = connStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var pwdR = pwdList.Where(w => w.ToLower().IndexOf("password") > 0);
                var pwdOriginal = pwdR.First().Substring(pwdR.First().IndexOf("=") + 1).Trim();
                var pwdNew = CryptographyHelper.Decrypt(pwdOriginal);
                connStr = connStr.Replace(pwdOriginal, pwdNew);
            }

            conn = new CrmServiceClient(connStr);
            var serviceProxy = conn.OrganizationServiceProxy;
            var proxyClient = conn.OrganizationWebProxyClient;

            if (serviceProxy != null)
            {
                serviceProxy.Timeout = new TimeSpan(0, 5, 0);
                organizationService = (IOrganizationService)serviceProxy;
                ConsoleHelper.Log("CRM > Authentication OK");
            }
            else if (proxyClient != null)
            {
                organizationService = (IOrganizationService)proxyClient;
                ConsoleHelper.Log("CRM > Authentication OK");
            } 
            else
            {
                ConsoleHelper.Log("CRM > Authentication > Not Authenticated");
                throw new Exception("CRM > Authentication > Not Authenticated");
            }
            ConsoleHelper.Log("CRM > Authentication > Finished");
        }
    }
}
