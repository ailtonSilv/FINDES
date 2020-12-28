using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FINDES.Plugin
{
    public class StringHelper
    {
        public static string CreateJSONString(Entity entity, Dictionary<string, Tuple<string, string>> fields, ref string json, IOrganizationService service)
        {
            var error = "";
            var fieldName = "";

            try {
                foreach (var field in fields) {
                    fieldName = field.Value.Item1;
                    var fieldKey = field.Key;
                    var fieldKeyGetById = false;
                    if (fieldKey.IndexOf("(id)") > 0) {
                        fieldKeyGetById = true;
                        fieldKey = fieldKey.Replace("(id)", "");
                    }

                    if (!entity.Attributes.ContainsKey(fieldKey)) continue;
                    var fieldValue = "";
                    var fieldType = "";
                    switch (field.Value.Item2) {
                        case "guid":
                            fieldType = "string";
                            fieldValue = entity.GetAttributeValue<Guid>(fieldKey).ToString();
                            break;
                        case "lookup":
                            fieldType = "string";
                            if (fieldKeyGetById && fieldName == fieldKey.Replace("(id)", ""))
                                { fieldValue = entity.GetAttributeValue<EntityReference>(fieldKey).Id.ToString(); }
                            else if (fieldKeyGetById && fieldName != fieldKey.Replace("(id)", ""))
                            {
                                Entity ett = service.Retrieve(entity.GetAttributeValue<EntityReference>(fieldKey).LogicalName, entity.GetAttributeValue<EntityReference>(fieldKey).Id, new Microsoft.Xrm.Sdk.Query.ColumnSet(new string[] { fieldName }));
                                if (ett != null && ett.Contains(fieldName))
                                {
                                    fieldValue = ett[fieldName].ToString();
                                }
                                ett = null;
                            }
                            else
                                { fieldValue = entity.GetAttributeValue<EntityReference>(fieldKey).Name; }
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
                            fieldValue = entity.GetAttributeValue<int>(fieldKey).ToString("N0").Replace(",","").Replace(".","");
                            break;
                        case "bool":
                        case "boolean":
                            fieldType = "boolean";
                            fieldValue = entity.GetAttributeValue<bool>(fieldKey).ToString();
                            break;
                        case "decimal":
                            fieldType = "decimal";
                            if (fieldKey == "quantity") {
                                fieldValue = Decimal.ToInt32(entity.GetAttributeValue<decimal>(fieldKey)).ToString();
                            } else {
                                fieldValue = entity.GetAttributeValue<decimal>(fieldKey).ToString("N2");
                            }
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
                    if (!string.IsNullOrEmpty(fieldValue)) {
                        if (fieldKeyGetById && fieldName != fieldKey.Replace("(id)", "")) 
                            json += "\"" + fieldKey.Replace("(id)", "") + "\":{";
                        else
                            json += "\"" + fieldName + "\":{";

                        json += "\"type\":\"" + fieldType + "\",";
                        json += "\"value\":\"" + fieldValue + "\"";
                        json += "},";
                    }
                }
            }
            catch (Exception ex) {
                error = ex.Message + " (" + fieldName + ")";
            }

            return error;
        }
    }
}
