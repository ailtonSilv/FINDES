using Findes.Standard.Core.Util;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Findes.WebApi
{
    public class StringHelper
    {
        IOptions<AppSettings> AppSettings;
        public StringHelper(IOptions<AppSettings> appSetttings) {
            AppSettings = appSetttings;
        }

        public string CreateJSONString(List<JsonFieldMapping> fields, ref string json) {
            var error = "";
            var fieldName = "";

            try {
                foreach (var field in fields) {
                    fieldName = field.AttributeName;
                    var fieldValue = "";
                    var fieldType = "";
                    var fieldContent = (field.Content !=  null ? field.Content.ToString() : "");
                    var searchfield = "";
                    var lookupEntityName = "";
                    var ismultilookup = "false";
                    var activityPartyFrom_EntityId = "";
                    var activityPartyFrom_EntityName = "";
                    var activityPartyTo_EntityId = "";
                    var activityPartyTo_EntityName = "";

                    switch (field.Type) {
                        case "activityparty":
                            fieldType = "activityparty";
                            fieldValue = fieldType;
                            activityPartyFrom_EntityId = field.ActivityPartyFrom_EntityId;
                            activityPartyFrom_EntityName = field.ActivityPartyFrom_EntityName;
                            activityPartyTo_EntityId = field.ActivityPartyTo_EntityId;
                            activityPartyTo_EntityName = field.ActivityPartyTo_EntityName;
                            break;
                        case "lookup":
                            fieldType = "lookup";
                            fieldValue = fieldContent;
                            searchfield = field.LookupSearchField;
                            lookupEntityName = field.LookupEntityName;
                            ismultilookup = field.IsMultiLookup.ToString().ToLower();
                            break;
                        case "guid":
                        case "string":
                            fieldType = "string";
                            fieldValue = fieldContent;
                            break;
                        case "optionset":
                            fieldType = "int";
                            fieldValue = fieldContent;
                            break;
                        case "int":
                        case "integer":
                            fieldType = "int";
                            fieldValue = int.Parse(fieldContent).ToString("N0");
                            break;
                        case "bool":
                        case "boolean":
                            fieldType = "boolean";
                            fieldValue = fieldContent;
                            break;
                        case "decimal":
                            fieldType = "decimal";
                            fieldValue = decimal.Parse(fieldContent).ToString("N2");
                            break;
                        case "money":
                            fieldType = "money";
                            fieldValue = decimal.Parse(fieldContent).ToString("N2");
                            break;
                        case "datetime":
                            fieldType = "datetime";
                            fieldValue = DateTime.Parse(fieldContent).ToString("yyyy/MM/dd HH:mm:ss");
                            break;
                    }
                    if (!string.IsNullOrEmpty(fieldValue)) {
                        json += "\"" + fieldName + "\":{";
                        json += "\"type\":\"" + fieldType + "\",";
                        json += "\"value\":\"" + fieldValue + "\",";
                        json += "\"searchfield\":\"" + searchfield + "\",";
                        json += "\"entityname\":\"" + lookupEntityName + "\",";
                        json += "\"ismultilookup\":\"" + ismultilookup + "\",";
                        json += "\"activitypartyfrom_entityid\":\"" + activityPartyFrom_EntityId + "\",";
                        json += "\"activitypartyfrom_entityname\":\"" + activityPartyFrom_EntityName + "\",";
                        json += "\"activitypartyto_entityid\":\"" + activityPartyTo_EntityId + "\",";
                        json += "\"activitypartyto_entityname\":\"" + activityPartyTo_EntityName + "\"";
                        json += "},";
                    }
                }

                json = json.Substring(0, json.Length - 1);
                json = "{" + json + "}";
            }
            catch (Exception ex) {
                error = ex.Message + " (" + fieldName + ") - Detail: " + ex.StackTrace;
            }

            return error;
        }

        public bool IsValidUser(JObject jsonParam) {
            var username = jsonParam["username"].ToString().ToLower();
            var password = CryptographyHelper.Decrypt(jsonParam["password"].ToString());
            var appSettingsPwd = CryptographyHelper.Decrypt(AppSettings.Value.password);
            return (AppSettings.Value.username.ToLower().Equals(username) && appSettingsPwd.Equals(password));
        }

    }
}
