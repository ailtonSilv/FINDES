using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FINDES.Plugin
{
    public class CRMHelper
    {
        public static Entity getEntity(IOrganizationService service, string entityName, string[] columns, string fieldName, Guid fieldId, ref string getEntityResultError)
        {
            var _entity = new Entity();
            try {
                var query = new QueryByAttribute();
                query.ColumnSet = new ColumnSet(columns);
                query.EntityName = entityName;
                query.Attributes.Add(fieldName);
                query.Values.Add(fieldId);

                var request = new RetrieveMultipleRequest();
                request.Query = query;
                var entityList = ((RetrieveMultipleResponse)service.Execute(request)).EntityCollection.Entities;

                if (entityList.Count() > 0) _entity = entityList[0];
            }
            catch (Exception ex) {
                getEntityResultError = ex.Message;
            }

            return _entity;
        }

        public static Entity getEntity(IOrganizationService service, FilterExpression filter, string entityName, string[] columns, ref string getEntityResultError)
        {
            var _entity = new Entity();
            try {
                var query = new QueryExpression(entityName);
                query.ColumnSet = new ColumnSet(columns);
                query.Criteria.AddFilter(filter);

                var request = new RetrieveMultipleRequest();
                request.Query = query;
                var entityList = ((RetrieveMultipleResponse)service.Execute(request)).EntityCollection.Entities;

                if (entityList.Count() > 0) _entity = entityList[0];
            }
            catch (Exception ex) {
                getEntityResultError = ex.Message;
            }

            return _entity;
        }

        public static EntityCollection getEntityCollection(IOrganizationService service, FilterExpression filter, string entityName, string[] columns, ref string getEntityResultError)
        {
            var _entityColl = new EntityCollection();
            try {
                var query = new QueryExpression(entityName);
                query.ColumnSet = new ColumnSet(columns);
                query.Criteria.AddFilter(filter);

                var request = new RetrieveMultipleRequest();
                request.Query = query;
                _entityColl = ((RetrieveMultipleResponse)service.Execute(request)).EntityCollection;
            }
            catch (Exception ex) {
                getEntityResultError = ex.Message;
            }

            return _entityColl;
        }

        public static List<Tuple<string, string>> ParseXMLResult(string methodName, string serviceNamespace, string soapResponse)
        {
            var response = new List<Tuple<string, string>>();

            try {
                var soapXMLResponse = XDocument.Parse(soapResponse);
                var elementName = XName.Get(methodName, serviceNamespace);

                var selectedElements = from elements in soapXMLResponse.Root.Descendants()
                                       where (elements.Name == elementName)
                                       select elements;

                var row = 1;
                foreach (var elem in selectedElements.Nodes()) {
                    var xmlNode = elem.ToString();
                    var xml = XDocument.Parse(xmlNode);
                    var hasDescendants = xml.Root.Descendants().Count() > 0;
                    ParseXMLElement(xml, hasDescendants, serviceNamespace, ref row, ref response);
                }
            }
            catch (Exception ex) {
            }

            return response;
        }

        private static void ParseXMLElement(XDocument xml,
                                          bool hasDescendants,
                                          string serviceNamespace,
                                          ref int row,
                                          ref List<Tuple<string, string>> response)
        {
            var _list = new List<Tuple<string, string>>();

            if (hasDescendants) {
                foreach (var elem in xml.Root.Descendants()) {
                    var rootName = xml.Root.Name.LocalName.Replace("{" + serviceNamespace + "}", "");
                    var name = elem.Name.LocalName;
                    var value = elem.Value;
                    var _tuple = new Tuple<string, string>(rootName + "_" + name, value);
                    response.Add(_tuple);
                }
            } else {
                var name = xml.Root.Name.LocalName.Replace("{" + serviceNamespace + "}", "");
                var value = xml.Root.Value;
                var _tuple = new Tuple<string, string>(name, value);
                response.Add(_tuple);
            }

        }

    }
}
