﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FINDES.Plugin.CRMFields;
using FINDES.Plugin;
using FINDES.ConsoleCarga.Util;
using System.Text;

namespace FINDES.ConsoleCarga
{
    public class Regional
    {
        public bool error = false;
        protected static string file = $"Regional_{DateTime.Now.ToShortDateString().Replace("/", "")}-{DateTime.Now.ToShortTimeString().Replace(":", "")}.csv";

        /// <summary>
        /// Export Regional
        /// </summary>
        /// <param name="method">CSV; WEBAPI</param>
        /// <param name="onlyCargaInicial">True = Filter by CargaInicial; False = all items</param>
        /// <param name="testMode">True = TopCount 10; False = all items</param>
        public static void Send(string method, bool onlyCargaInicial, bool testMode)
        {
            try
            {
                ConsoleHelper.Log("");
                ConsoleHelper.Log("Processing > Regional > Started");
                ConsoleHelper.Log($"Method: {method}.");

                var fields = Fields.Regional.fields;
                int pageNumber = 1;

                #region | QueryExpression
                var query = new QueryExpression(Fields.Regional.entityName);
                query.ColumnSet = new ColumnSet(fields.Select(s => s.Key.Replace("(id)", "")).ToArray());
                query.Criteria.AddFilter(LogicalOperator.And);
                if (onlyCargaInicial)
                {
                    ConsoleHelper.Log("Listing filter by 'findes_cargainicial' = 'sim'.");
                    query.Criteria.AddFilter(LogicalOperator.Or);
                    query.Criteria.Filters[1].AddCondition("findes_cargainicial", ConditionOperator.Null);
                    query.Criteria.Filters[1].AddCondition("findes_cargainicial", ConditionOperator.Equal, false);
                }

                query.Orders.Add(new OrderExpression("createdon", OrderType.Ascending));

                if (testMode)
                {
                    ConsoleHelper.Log("**************************************************");
                    ConsoleHelper.Log("TestMode, only first 10 records will be proccessed.");
                    ConsoleHelper.Log("**************************************************");
                    query.TopCount = 10;
                }
                else
                {
                    ConsoleHelper.Log("TestMode = false.");
                    query.PageInfo = new PagingInfo
                    {
                        Count = 1000,
                        PageNumber = pageNumber,
                        PagingCookie = null
                    };
                }
                #endregion

                #region | PROCESS
                ConsoleHelper.ProcessQuery(method, query, fields, file, Fields.Regional.tableSQL, Fields.Regional.entityName);
                #endregion

                ConsoleHelper.Log("Processing > Regional > Finished");
            }
            catch (Exception ex)
            {
                ConsoleHelper.Log(ex.Message);
                ConsoleHelper.Log(ex.StackTrace);
            }
        }
    }
}