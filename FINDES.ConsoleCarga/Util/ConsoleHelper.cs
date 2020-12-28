using FINDES.Plugin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Xrm.Sdk.Query;

namespace FINDES.ConsoleCarga.Util
{
    public class ConsoleHelper
    {
        protected static int line = 0;
        protected static int processedTotal = 0;
        public static void Log(string text)
        {
            line++;
            string msg = $"{line.ToString()} - {DateTime.Now.ToString("G")} => {text}";
            Console.WriteLine(msg);
            if (line == 1)
            {
                AppendLogError("====================================================");
            }
            AppendLogError(msg);
        }

        public static void AppendLogError(string erro)
        {
            try
            {
                string appDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().CodeBase).Replace(@"file:\", "") + @"\";
                StreamWriter sw = File.AppendText(string.Concat(appDir, "logError.txt"));
                sw.WriteLine(erro);
                sw.Dispose();
            }
            catch { }
        }

        protected static int lineCsv = 0;
        public static void AppendCSV(string text, string file)
        {
            try
            {
                string appDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().CodeBase).Replace(@"file:\", "") + @"\";
                //pFile.AppendText(string.Concat(appDir, file))
                StreamWriter sw = new StreamWriter(string.Concat(appDir, file), true, Encoding.GetEncoding("iso-8859-1"));
                //StreamWriter sw = File.AppendText(string.Concat(appDir, file));
                sw.WriteLine(text);
                sw.Dispose();
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        public static void GenerateCSVHeader(Dictionary<string, Tuple<string, string>> fields, string file)
        {
            try
            {
                var orderned = fields.OrderBy(x => x.Value);
                StringBuilder sb = new StringBuilder();
                foreach (var field in orderned)
                {
                    string fieldName = field.Value.Item1;
                    var fieldKey = field.Key;
                    if (fieldKey.IndexOf("(id)") > 0)
                    {
                        fieldKey = fieldKey.Replace("(id)", "");
                    }
                    sb.Append($"{fieldName}; ");
                }
                ConsoleHelper.AppendCSV(sb.ToString(), file);
            }
            catch (Exception ex)
            {
                ConsoleHelper.Log(ex.Message);
            }
        }

        protected static int origRow;
        protected static int origCol;

        public static void SetPosition()
        {
            origRow = Console.CursorTop;
            origCol = Console.CursorLeft;
        }

        public static void ShowProgress(string s)
        {
            Console.SetCursorPosition(0, origRow);
            Console.CursorVisible = false;
            if (s == "100,00 %")
            {
                Console.WriteLine(s, s.Length+1);

            }
            else
            {
                Console.Write(s, s.Length);
            }
            
        }

        public static void GenerateCSV(List<Entity> listEntities, Dictionary<string, Tuple<string, string>> fields, string file)
        {
            try
            {
                var orderned = fields.OrderBy(x => x.Value);
                StringBuilder sb = new StringBuilder();

                string fieldName;
                int item = 0;
                SetPosition();
                foreach (Entity entity in listEntities)
                {
                    item++;
                    double percentage = ((double)item / listEntities.Count) * 100;
                    ShowProgress($"{string.Format("{0:F2}", percentage)} %");
                    sb = new StringBuilder();
                    foreach (var field in orderned)
                    {
                        var fieldValue = " ";
                        var fieldType = "";
                        fieldName = "";

                        fieldName = field.Value.Item1;
                        var fieldKey = field.Key;
                        var fieldKeyGetById = false;
                        if (fieldKey.IndexOf("(id)") > 0)
                        {
                            fieldKeyGetById = true;
                            fieldKey = fieldKey.Replace("(id)", "");
                        }
                        if (!entity.Attributes.ContainsKey(fieldKey))
                        {
                            sb.Append($" ; ");
                            continue;
                        }

                        switch (field.Value.Item2)
                        {
                            case "guid":
                                fieldType = "string";
                                fieldValue = entity.GetAttributeValue<Guid>(fieldKey).ToString();
                                break;
                            case "lookup":
                                fieldType = "string";
                                fieldValue = entity.GetAttributeValue<EntityReference>(fieldKey).Id.ToString();
                                break;
                            case "optionset":
                                fieldType = "int";
                                fieldValue = entity.GetAttributeValue<OptionSetValue>(fieldKey).Value.ToString();
                                break;
                            case "string":
                                fieldType = "string";
                                fieldValue = entity.GetAttributeValue<string>(fieldKey);
                                break;
                            case "int":
                            case "integer":
                                fieldType = "int";
                                fieldValue = entity.GetAttributeValue<int>(fieldKey).ToString("N0");
                                break;
                            case "bool":
                            case "boolean":
                                fieldType = "boolean";
                                fieldValue = entity.GetAttributeValue<bool>(fieldKey).ToString();
                                break;
                            case "decimal":
                                fieldType = "decimal";
                                fieldValue = entity.GetAttributeValue<decimal>(fieldKey).ToString("N2");
                                break;
                            case "money":
                                fieldType = "money";
                                fieldValue = entity.GetAttributeValue<Money>(fieldKey).Value.ToString("N2");
                                break;
                            case "datetime":
                                fieldType = "datetime";
                                fieldValue = entity.GetAttributeValue<DateTime>(fieldKey).ToString("yyyy/MM/dd HH:mm:ss");
                                break;
                        }
                        sb.Append($"{fieldValue}; ");
                    }
                    ConsoleHelper.AppendCSV(sb.ToString(), file);
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.Log(ex.Message);
            }
        }

        public static void PrepareSendtoAPI(IOrganizationService service, string ax4bConnectorWebApiURL, string tableDestination, Entity entity, Dictionary<string, Tuple<string, string>> sourceFields, out bool error)
        {
            error = false;
            try
            {
                var getEntityResultError = "";
                var fields = sourceFields;

                //if (!account.Attributes.ContainsKey("findes_codigoerp")) return;

                // PREPARA O JSON
                var json = "{";
                json += "\"entityname\": \"" + tableDestination + "\",";
                json += "\"sendto\": \"SQLServer\",";
                json += "\"records\":[{";

                getEntityResultError = StringHelper.CreateJSONString(entity, fields, ref json, service);
                if (!string.IsNullOrEmpty(getEntityResultError))
                {
                    ConsoleHelper.Log("PrepareSendtoAPI: An error occurred in 'CreateJSONString': " + getEntityResultError);
                    error = true;
                    return;
                }

                json += "\"findes_dataatualizacao\":{";
                json += "\"type\":\"datetime\",";
                json += "\"value\":\"" + DateTime.Now.AddHours(-2).ToString("yyyy/MM/dd HH:mm:ss") + "\"";
                json += "}";
                json += "}]";
                json += "}";

                // ENVIA PARA O STAGING
                var result = RESTHelper.PostRawJson(ax4bConnectorWebApiURL, json);
                var oResult = JsonHelper<ResultModel>.DeSerialize(result);

                if (!string.IsNullOrEmpty(oResult.status) && oResult.status != "Success")
                {
                    ConsoleHelper.Log("PrepareSendtoAPI: An error occurred in 'PostRawJson': " + oResult.message);
                    error = true;
                }
            }
            catch (Exception ex)
            {
                error = true;
                Log("PrepareSendtoAPI ERROR = " + ex.Message);
            }

            
        }

        public static void Process(List<Entity> oppList, string tableSQL, string entityName, Dictionary<string, Tuple<string, string>> sourceFields)
        {
            List<Guid> idsOk = new List<Guid>();

            #region | Loop Itens
            int item = 1;
            foreach (Entity ett in oppList)
            {
                ConsoleHelper.Log(string.Format("Sending to WEBAPI {0} of {1}; Total sended {2}", item++, oppList.Count, processedTotal));

                // Call Method to load entity fields and send to API
                ConsoleHelper.PrepareSendtoAPI(Load.organizationService, Load.ax4bConnectorWebApiURL, tableSQL, ett, sourceFields, out bool executionError);

                // If no error during execution, update flag to true
                if (!executionError)
                {
                    //idsOk.Add(ett.Id);
                    Load.CheckCRMConnection();
                    Entity entity = new Entity(entityName, ett.Id);
                    entity["findes_cargainicial"] = true;
                    Load.conn.Update(entity);
                    processedTotal++;
                }
            }
            #endregion

            #region | Update Itens
            item = 1;
            ExecuteMultipleRequest multipleRequest = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = false,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };

            foreach (Guid id in idsOk)
            {
                Entity entity = new Entity(entityName, id);
                entity["findes_cargainicial"] = true;

                UpdateRequest updateRequest = new UpdateRequest { Target = entity };
                multipleRequest.Requests.Add(updateRequest);
            }
            if (idsOk.Count > 0)
            {
                Log("Send Update package to CRM...");
                ExecuteMultipleResponse responseWithResults = (ExecuteMultipleResponse)Load.conn.Execute(multipleRequest);
                item = 1;
                foreach (var responseItem in responseWithResults.Responses)
                {
                    //processedTotal++;
                    // A valid response.
                    if (responseItem.Response != null)
                    {
                        //DisplayResponse(multipleRequest.Requests[responseItem.RequestIndex], responseItem.Response);
                        ConsoleHelper.Log(string.Format("Update with success on item of CRM {0} of {1}", item++, responseWithResults.Responses.Count));
                    }

                    // An error has occurred.
                    else if (responseItem.Fault != null)
                    {
                        //DisplayFault(multipleRequest.Requests[responseItem.RequestIndex], responseItem.RequestIndex, responseItem.Fault);
                        ConsoleHelper.Log(string.Format("Update returned error on item of CRM {0} of {1}, error: {2}", item++, responseWithResults.Responses.Count, responseItem.Fault.Message));
                    }

                }
            }
  
            
            Log($"Total processed so far {processedTotal.ToString()}");
            #endregion
        }

        public static void ProcessQuery(string method, QueryExpression query, Dictionary<string, Tuple<string, string>> sourceFields, string file, string tableSQL, string entityName)
        {
            int pageNumber = 1;
            #region | Load Pages
            if (method == "CSV")
            {
                ConsoleHelper.GenerateCSVHeader(sourceFields, file);
            }
            EntityCollection ret = new EntityCollection();
            while (true)
            {
                Load.CheckCRMConnection();
                List<Entity> oppList = new List<Entity>();
                if (pageNumber == 1)
                {
                    ret = Load.organizationService.RetrieveMultiple(query);
                    oppList = ret.Entities.ToList();
                    Console.WriteLine("");
                    ConsoleHelper.Log($"Processing Page: {query.PageInfo.PageNumber.ToString()}");
                    switch (method)
                    {
                        case "CSV":
                            ConsoleHelper.GenerateCSV(oppList, sourceFields, file);
                            break;
                        case "WEBAPI":
                            ConsoleHelper.Process(oppList, tableSQL, entityName, sourceFields);
                            break;
                    }
                }


                if (ret.MoreRecords)
                {
                    Load.CheckCRMConnection();
                    pageNumber++;
                    query.PageInfo.PageNumber = pageNumber;
                    query.PageInfo.PagingCookie = ret.PagingCookie;
                    ret = Load.organizationService.RetrieveMultiple(query);
                    oppList = new List<Entity>();
                    oppList = ret.Entities.ToList();
                    Console.WriteLine("");
                    ConsoleHelper.Log($"Processing Page: {query.PageInfo.PageNumber.ToString()}");
                    switch (method)
                    {
                        case "CSV":
                            ConsoleHelper.GenerateCSV(oppList, sourceFields, file);
                            break;
                        case "WEBAPI":
                            ConsoleHelper.Process(oppList, tableSQL, entityName, sourceFields);
                            break;
                    }
                }
                else
                {
                    break;
                }

            }
            #endregion
        }
    }
}
