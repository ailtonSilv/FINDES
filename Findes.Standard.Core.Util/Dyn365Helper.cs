using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Findes.Standard.Core.Util
{
    public class Dyn365Helper
    {
        private static HttpClient httpClient;
        private static Version webAPIVersion = new Version(8, 0);
        private static string webApiURI = "";

        private static string ADURL;
        private static string client_id;
        private static string client_secret;
        private static string username;
        private static string password;
        private static string grant_type;
        private static string serverUri;
        public static AuthenticationResult AuthResult;
        private static string AccessToken;
        private static DateTime TokenRenewNotBefore;
        private static string RefreshToken;

        public Dyn365Helper(IOptions<AppSettings> appSettings) {
            try {
                ADURL = appSettings.Value.ADURL;
                serverUri = appSettings.Value.Dyn365URL;
                client_id = appSettings.Value.client_id;
                client_secret = appSettings.Value.client_secret;
                username = appSettings.Value.username;
                password = appSettings.Value.password;
                grant_type = appSettings.Value.grant_type;

                httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(serverUri + "/api/data/v8.1/");
                httpClient.Timeout = new TimeSpan(0, 2, 0);
                httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            catch (Exception ex) {
                ErrorHelper.CatchError(ex);
            }
        }

        public async Task<string> Authenticate() {
            var errorMessage = "";
            try
            {
                var _params = new Dictionary<string, object>() {
                    { "client_id", client_id },
                    { "client_secret", client_secret },
                    { "resource", serverUri },
                    { "username", username },
                    //{ "password", CryptographyHelper.Decrypt(password) },
                    { "password", password },
                    { "grant_type", grant_type }
                };

                var token = await ExecutePostAsync(ADURL, _params);
                var _jsonToken = (JObject)JsonConvert.DeserializeObject(token);

                if (token.Contains("error_description")) {
                    errorMessage = _jsonToken.SelectToken("error_description").ToString();
                } else {
                    TokenRenewNotBefore = DateTime.Now.AddSeconds(10);
                    AccessToken = _jsonToken["access_token"].ToString();
                    RefreshToken = _jsonToken["refresh_token"].ToString();
                }
            }
            catch (System.Exception ex) {
                errorMessage = ex.Message;
                ErrorHelper.CatchError(ex);
            }

            return errorMessage;
        }

        public void CallRefreshToken()
        {
            if (string.IsNullOrEmpty(RefreshToken))
                throw new Exception("Unauthorized access to Dynamics CRM. Please review credentials.");

            var _params = new Dictionary<string, object>() {
                    { "client_id", client_id },
                    { "client_secret", client_secret },
                    { "resource", serverUri },
                    { "grant_type", "refresh_token" },
                    { "refresh_token", RefreshToken }
                };

            var token = ExecutePostAsync(ADURL, _params).Result;
            var _jsonToken = (JObject)JsonConvert.DeserializeObject(token);
            TokenRenewNotBefore = DateTime.Now.AddSeconds(double.Parse(_jsonToken["not_before"].ToString()));
            AccessToken = _jsonToken["access_token"].ToString();
            RefreshToken = _jsonToken["refresh_token"].ToString();
        }

        public void getWebAPIVersion() {
            var rvRequest = new HttpRequestMessage(HttpMethod.Get, "RetrieveVersion");
            rvRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            HttpResponseMessage rvResponse = httpClient.SendAsync(rvRequest).Result;

            if (rvResponse.StatusCode == HttpStatusCode.OK) {
                var RetrievedVersion = JsonConvert.DeserializeObject<JObject>(rvResponse.Content.ReadAsStringAsync().Result);
                webAPIVersion = Version.Parse((string)RetrievedVersion.GetValue("Version"));
            }
        }

        public string getVersionedWebAPIPath() {
            getWebAPIVersion();
            return string.Format("v{0}/", webAPIVersion.ToString(2));
        }

        public async Task<HttpResponseMessage> SendCrmRequestAsync(string webApiURI, JObject jsonParam)
        {
            CheckToken();

            HttpClient actionClient = new HttpClient();
            actionClient.BaseAddress = new Uri(webApiURI);
            actionClient.Timeout = new TimeSpan(0, 2, 0);
            actionClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Post;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            if (jsonParam != null)
            {
                var jsonApp = JsonConvert.SerializeObject(jsonParam);
                var content = new StringContent(jsonApp, Encoding.UTF8, "application/json");
                request.Content = content;
            }

            var response = await actionClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                CallRefreshToken();
                return await SendCrmRequestAsync(webApiURI, jsonParam);
            }

            return response;
        }


        public async Task<HttpResponseMessage> SendCrmRequestAsync(HttpMethod method, string query, StringContent strContent = null, Boolean formatted = false, int maxPageSize = 100) {
            CheckToken();
            var request = new HttpRequestMessage();
            request.Method = method;
            request.RequestUri = new Uri(query);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            request.Headers.Add("Prefer", "odata.metadata=minimal");
            //request.Headers.Add("Prefer", "odata.maxpagesize=" + maxPageSize.ToString());
            if (formatted)
                request.Headers.Add("Prefer", "odata.include-annotations=OData.Community.Display.V1.FormattedValue");

            if (strContent != null) {
                request.Content = strContent;
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                CallRefreshToken();
                return await SendCrmRequestAsync(method, query, strContent, formatted, maxPageSize);
            }

            return response;
        }

        private void CheckToken()
        {
            if ((DateTime.Now > TokenRenewNotBefore))
                CallRefreshToken();
        }

        public string RemoveURLPrefix(string url) {
            return url.Replace("http://", "").Replace("http:\\", "").Replace("https://", "").Replace("https:\\", "");
        }

        private string ConvertFileFromBase64ToString(string fileContent) {
            var bytes = Convert.FromBase64String(fileContent);
            var ms = new MemoryStream(bytes);
            var sr = new StreamReader(ms, Encoding.UTF7);
            var strFileContent = sr.ReadToEnd();
            ms.Dispose();
            sr.Dispose();
            return strFileContent;
        }

        public CustomResult CreateRecord(string entityName, JToken record, ref string generalError) {
            var webApiURI = serverUri + "/api/data/" + getVersionedWebAPIPath();
            var objBody = PrepareObjBody(entityName, record, ref generalError);
            var jsonApp = JsonConvert.SerializeObject(objBody);
            var content = new StringContent(jsonApp, Encoding.UTF8, "application/json");
            var response = SendCrmRequestAsync(HttpMethod.Post, webApiURI + getEntityPluralName(entityName), content).Result;
            var result = ParseResult(response.Content.ReadAsStringAsync().Result, "", "CreateRecord (" + entityName + ")");

            if (result.Status.Equals(100)) {
                if (!response.Headers.Contains("OData-EntityId"))
                    throw new Exception($"Unexpected behaviour. Received response with Status 100 { response } and content { response.Content.ReadAsStringAsync().Result }");

                var OData_EntityId = response.Headers.GetValues("OData-EntityId").FirstOrDefault();
                OData_EntityId = OData_EntityId.Substring(OData_EntityId.IndexOf("(") + 1).Replace(")", "");
                result.Status = 100;
                result.Id = OData_EntityId;
            } else {
                generalError = result.Message;
            }

            return result;
        }

        public CustomResult UpdateRecord(string entityName, JToken record, string recordId, ref string generalError) {
            var result = new CustomResult() { Status = 100 };

            var webApiURI = serverUri + "/api/data/" + getVersionedWebAPIPath();
            var objBody = PrepareObjBody(entityName, record, ref generalError);
            var jsonApp = JsonConvert.SerializeObject(objBody);
            var content = new StringContent(jsonApp, Encoding.UTF8, "application/json");
            var response = SendCrmRequestAsync(new HttpMethod("PATCH"), webApiURI + getEntityPluralName(entityName) + "(" + recordId + ")", content).Result;
            result = ParseResult(response.Content.ReadAsStringAsync().Result, "", "UpdateRecord (" + entityName + ")");

            if (result.Status.Equals(100))
            {
                if (!response.Headers.Contains("OData-EntityId"))
                    throw new Exception($"Unexpected behaviour. Received response with Status 100 { response } and content { response.Content.ReadAsStringAsync().Result }");

                var OData_EntityId = response.Headers.GetValues("OData-EntityId").FirstOrDefault();
                OData_EntityId = OData_EntityId.Substring(OData_EntityId.IndexOf("(") + 1).Replace(")", "");
                result.Status = 100;
                result.Id = OData_EntityId;
            }
            else
            {
                generalError = result.Message;
            }

            return result;
        }

        public CustomResult UpdateImageRecord(string entityName, string recordId, string imageContent, ref string generalError) {
            var result = new CustomResult() { Status = 100 };

            var webApiURI = serverUri + "/api/data/" + getVersionedWebAPIPath();
            var jsonApp = "{\"value\":\"" + imageContent + "\"}";
            var content = new StringContent(jsonApp, Encoding.UTF8, "application/json");
            var response = SendCrmRequestAsync(new HttpMethod("PATCH"), webApiURI + getEntityPluralName(entityName) + "(" + recordId + ")/entityimage", content).Result;
            result = ParseResult(response.Content.ReadAsStringAsync().Result, "", "UpdateImageRecord (" + entityName + ")");

            return result;
        }

        public Guid WhoAmI() {
            var userId = Guid.Empty;
            var webApiURI = serverUri + "/api/data/" + getVersionedWebAPIPath();
            var response = SendCrmRequestAsync(HttpMethod.Get, webApiURI + "WhoAmI()", null, true).Result;
            if (response.StatusCode == HttpStatusCode.OK) {
                var result = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                userId = Guid.Parse(result["UserId"].ToString());
            }
            return userId;
        }

        public void CreateAnnotation(string subject, string fileName, string fileContent, string entityReferenceName, string entityReferenceId) {
            var jsonValue = "{";
            jsonValue += "\"subject\":\"" + subject + "\",";
            jsonValue += "\"filename\":\"" + fileName + "\",";
            jsonValue += "\"objectid_" + entityReferenceName + "@odata.bind\":\"/" + getEntityPluralName(entityReferenceName) + "(" + entityReferenceId + ")\",";

            if (fileContent.IndexOf("base64,") > 0) {
                fileContent = fileContent.Substring(fileContent.IndexOf("base64,") + 7);
            }

            jsonValue += "\"documentbody\":\"" + fileContent + "\"";
            jsonValue += "}";

            var result = new CustomResult() { Status = 100 };
            var webApiURI = serverUri + "/api/data/" + getVersionedWebAPIPath();
            var content = new StringContent(jsonValue, Encoding.UTF8, "application/json");
            var response = SendCrmRequestAsync(HttpMethod.Post, webApiURI + "annotations", content).Result;
            result = ParseResult(response.Content.ReadAsStringAsync().Result, "", "CreateAnnotation");

        }

        private Dictionary<string, object> PrepareObjBody(string entityName, JToken record, ref string generalError) {
            var objBody = new Dictionary<string, object>();

            foreach (JProperty prop in JObject.Parse(record.ToString()).Properties()) {
                var type = record[prop.Name]["type"].ToString().ToLower();
                var value = record[prop.Name]["value"].ToString();
                if (string.IsNullOrEmpty(value)) continue;

                switch (type) {
                    case "activityparty":
                        var activitypartyfrom_entityname = record[prop.Name]["activitypartyfrom_entityname"].ToString();
                        var activitypartyfrom_entityid = record[prop.Name]["activitypartyfrom_entityid"].ToString();
                        var activitypartyto_entityname = record[prop.Name]["activitypartyto_entityname"].ToString();
                        var activitypartyto_entityid = record[prop.Name]["activitypartyto_entityid"].ToString();
                        value = "[{";
                        value += "\"partyid_" + activitypartyfrom_entityname + "@odata.bind\":\"" + "/" + getEntityPluralName(activitypartyfrom_entityname) + "(" + activitypartyfrom_entityid + ")\",";
                        value += "\"participationtypemask\":\"1\"";
                        value += "},";
                        value += "{";
                        value += "\"partyid_" + activitypartyto_entityname + "@odata.bind\":\"" + "/" + getEntityPluralName(activitypartyto_entityname) + "(" + activitypartyto_entityid + ")\",";
                        value += "\"participationtypemask\":\"2\"";
                        value += "}]";
                        objBody.Add(prop.Name, JsonConvert.DeserializeObject(value));
                        break;
                    case "boolean":
                    case "bool":
                        if (value == "0") value = "false";
                        if (value == "1") value = "true";
                        objBody.Add(prop.Name, value);
                        break;
                    case "date":
                        var dtOut = DateTime.ParseExact(value, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
                        if (dtOut != null)
                            objBody.Add(prop.Name, dtOut.Year + "-" + dtOut.Month.ToString().PadLeft(2, '0') + "-" + dtOut.Day.ToString().PadLeft(2, '0'));
                        break;
                    case "datetime":
                        var dtOut2 = DateTime.ParseExact(value, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
                        if (dtOut2 != null)
                            objBody.Add(prop.Name, dtOut2.Year + "-" + dtOut2.Month.ToString().PadLeft(2, '0') + "-" + dtOut2.Day.ToString().PadLeft(2, '0') + "T" + dtOut2.Hour.ToString().PadLeft(2, '0') + ":" + dtOut2.Minute.ToString().PadLeft(2, '0') + ":00Z");
                        break;
                    case "lookup":
                        var propName = prop.Name + "@odata.bind";
                        var searchField = record[prop.Name]["searchfield"].ToString();
                        var lookup_entityName = record[prop.Name]["entityname"].ToString();
                        var ismultiLookup = false;
                        if (record[prop.Name]["ismultilookup"] != null) {
                            ismultiLookup = bool.Parse(record[prop.Name]["ismultilookup"].ToString());
                            if (ismultiLookup) propName = prop.Name + "_" + lookup_entityName + "@odata.bind";
                        }

                        Guid gOut;
                        Guid.TryParse(value, out gOut);
                        if (gOut == Guid.Empty) {
                            value = getEntityIdByValue(lookup_entityName, searchField, value, ref generalError);
                        } else {
                            value = gOut.ToString();
                        }

                        if (!string.IsNullOrEmpty(value)) {
                            value = "/" + getEntityPluralName(lookup_entityName) + "(" + value.ToString() + ")";
                            objBody.Add(propName, value);
                        }
                        break;
                    case "money":
                    case "decimal":
                        objBody.Add(prop.Name, decimal.Parse(value));
                        break;
                    case "optionset":
                        objBody.Add(prop.Name, value.Replace(".", ""));
                        break;
                    default:
                        objBody.Add(prop.Name, value);
                        break;
                }
            }

            return objBody;
        }

        public string getEntityIdByValue(string entityName, string searchField, string value, ref string errorMsg) {
            var webApiURI = serverUri + "/api/data/" + getVersionedWebAPIPath();
            string resultID = "";
            var pluralEntityName = getEntityPluralName(entityName);
            var fieldId = entityName + "id";
            var queryOptions = pluralEntityName + "?$select=" + fieldId;
            queryOptions += "&$filter=" + getSearchField(entityName, searchField) + " eq " + $"'{value}'";

            var response = SendCrmRequestAsync(HttpMethod.Get, webApiURI + queryOptions, null, true).Result;
            var _result = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<JObject>(_result);
            if (result != null) {
                var jError = (JObject)result.SelectToken("error");
                var jValue = (JArray)result.SelectToken("value");
                if (jValue != null && jValue.Count > 0) resultID = jValue.First()[fieldId].ToString();
                if (jError != null) errorMsg = jError.SelectToken("message").ToString() + Environment.NewLine;
            }

            return resultID;
        }

        public string getEntityPluralName(string entityName) {
            var pluralEntityName = entityName + "s";
            if (entityName.Equals("territory")) pluralEntityName = "territories";
            return pluralEntityName;
        }

        public string getSearchField(string entityName, string searchField) {
            var nameField = searchField;

            if (string.IsNullOrEmpty(nameField)) {
                switch (entityName.ToLower()) {
                    case "subject":
                        nameField = "title";
                        break;
                    case "activitypointer":
                    case "campaignactivity":
                    case "serviceappointment":
                    case "socialactivity":
                    case "appointment":
                    case "recurringappointmentmaster":
                    case "email":
                    case "fax":
                    case "task":
                    case "phonecall":
                        nameField = "subject";
                        break;
                    case "list":
                        nameField = "listname";
                        break;
                    case "quotedetail":
                        nameField = "quotedetailname";
                        break;
                    case "invoicedetail":
                        nameField = "invoicedetailname";
                        break;
                    case "opportunityproduct":
                        nameField = "opportunityproductname";
                        break;
                    case "salesorderdetail":
                        nameField = "salesorderdetailname";
                        break;
                    case "systemuser":
                        nameField = "fullname";
                        break;
                }
            }

            return nameField;
        }

        public CustomResult ParseResult(string resultContent, string fieldId = "", string origin = "") {
            var result = new CustomResult() { Status = 100 };

            if (!string.IsNullOrEmpty(resultContent)) {
                var joResult = JsonConvert.DeserializeObject<JObject>(resultContent);
                if (joResult.SelectToken("error") != null) {
                    var message = JsonConvert.DeserializeObject<JObject>(resultContent).SelectToken("error")["message"].ToString();
                    result.Status = 600;
                    result.Message = origin + " - " + message;
                } else {
                    if (!string.IsNullOrEmpty(fieldId)) {
                        var _value = joResult.SelectToken("value");
                        if (_value.Count() > 0) {
                            result.Id = joResult.SelectToken("value")[0][fieldId].ToString();
                        }
                    }
                }
            }

            return result;
        }

        public async Task<string> ExecutePostAsync(string url, Dictionary<string, object> content) {
            string result = "";
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(5);

            try {
                if (content != null) {
                    var keyValues = new List<KeyValuePair<string, string>>();
                    foreach (var item in content) {
                        keyValues.Add(new KeyValuePair<string, string>(item.Key, item.Value.ToString()));
                    }
                    var formContent = new FormUrlEncodedContent(keyValues);

                    var request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = formContent;
                    result = await client.SendAsync(request).Result.Content.ReadAsStringAsync();
                }
                client.Dispose();
            }
            catch (Exception ex) {
                throw new Exception(ex.Message, ex.InnerException);
            }

            return result;
        }

        #region typedMethods
        public string CreateJSONString(List<Dyn365Attribute> fields)
        {
            var json = "";
            foreach (var field in fields)
            {
                var fieldName = field.AttributeName;
                var fieldValue = "";
                var fieldType = "";
                var fieldContent = (field.Content != null ? System.Web.HttpUtility.JavaScriptStringEncode(field.Content) : "");
                var searchfield = "";
                var lookupEntityName = "";
                var ismultilookup = "false";
                var activityPartyFrom_EntityId = "";
                var activityPartyFrom_EntityName = "";
                var activityPartyTo_EntityId = "";
                var activityPartyTo_EntityName = "";

                switch (field.Type)
                {
                    case CrmType.ActivityParty:
                        fieldType = "activityparty";
                        fieldValue = fieldType;
                        activityPartyFrom_EntityId = field.ActivityPartyFrom_EntityId;
                        activityPartyFrom_EntityName = field.ActivityPartyFrom_EntityName;
                        activityPartyTo_EntityId = field.ActivityPartyTo_EntityId;
                        activityPartyTo_EntityName = field.ActivityPartyTo_EntityName;
                        break;
                    case CrmType.Lookup:
                        fieldType = "lookup";
                        fieldValue = fieldContent;
                        searchfield = field.LookupSearchField;
                        lookupEntityName = field.LookupEntityName;
                        ismultilookup = field.IsMultiLookup.ToString().ToLower();
                        break;
                    case CrmType.String:
                        fieldType = "string";
                        fieldValue = fieldContent;
                        break;
                    case CrmType.OptionSet:
                        fieldType = "int";
                        fieldValue = fieldContent;
                        break;
                    case CrmType.Int:
                        fieldType = "int";
                        fieldValue = int.Parse(fieldContent).ToString("N0");
                        break;
                    case CrmType.Bool:
                        fieldType = "boolean";
                        fieldValue = fieldContent;
                        break;
                    case CrmType.Decimal:
                        fieldType = "decimal";
                        fieldValue = decimal.Parse(fieldContent).ToString("N2");
                        break;
                    case CrmType.Money:
                        fieldType = "money";
                        fieldValue = decimal.Parse(fieldContent).ToString("N2");
                        break;
                    case CrmType.DateTime:
                        if (string.IsNullOrEmpty(fieldContent))
                            break;
                        fieldType = "datetime";
                        if (DateTime.Parse(fieldContent) > DateTime.Parse("01/01/1753 00:00:00"))
                        {
                            fieldValue = DateTime.Parse(fieldContent).ToString("yyyy/MM/dd HH:mm:ss");
                        }
                        break;
                }
                if (!string.IsNullOrEmpty(fieldValue))
                {
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

            return json;
        }

        public CustomResult Upsert(Dyn365Entity entity)
        {
            if (entity.Id.HasValue)
                return Update(entity);

            return Create(entity);
        }

        public CustomResult Create(Dyn365Entity entity)
        {
            var just_a_string = "";
            return CreateRecord(entity.LogicalName, JsonConvert.DeserializeObject<JToken>(CreateJSONString(entity.Attributes)), ref just_a_string);
        }

        public CustomResult Update(Dyn365Entity entity)
        {
            var just_a_string = "";
            return UpdateRecord(entity.LogicalName, JsonConvert.DeserializeObject<JToken>(CreateJSONString(entity.Attributes)), entity.Id.ToString(), ref just_a_string);
        }

        public JToken Retrieve(string EntityName, string Attributes, string Filter)
        {
            if (string.IsNullOrEmpty(EntityName))
                throw new Exception($"Must provide entityName");

            if (string.IsNullOrEmpty(Attributes))
                throw new Exception($"Must provide at least one attribute");

            var webApiURI = serverUri + "/api/data/" + getVersionedWebAPIPath();

            var queryString = getEntityPluralName(EntityName);
            queryString += "?$select=" + Attributes;

            if (!string.IsNullOrWhiteSpace(Filter))
                queryString += "&$filter=" + Filter;

            var response = SendCrmRequestAsync(HttpMethod.Get, webApiURI + queryString, null, true).Result;

            try
            {
                return JToken.Parse(response.Content.ReadAsStringAsync().Result).SelectToken("value");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error parsing JSON. { response } The following string is not valid json. { response.Content.ReadAsStringAsync().Result }", ex);
            }
        }

        #endregion
    }


    #region classes

    public class Dyn365Query
    {
        //var webApiURI = this.AppSettings.Value.Dyn365URL + "/api/data/" + dyn365H.getVersionedWebAPIPath();

        //var colunas = "copasa_tipocliente,copasa_numerodoc,copasa_tipodocumento,copasa_cpf_cnpj,fullname,emailaddress1,telephone1,telephone2,mobilephone,address1_line1,address1_line2,address1_country,address1_line3,address1_postalcode&$expand=copasa_localidadeid($select=copasa_localidadeid, copasa_name),copasa_estadoid($select=copasa_estadoid, copasa_name)";

        //var queryOptions = "contacts?$select=" + colunas;
        //queryOptions += $"&$filter=copasa_cpf_cnpj eq '{cpfCnpj}'";
        public string Attributes { get; set; }
        public string EntityName { get; set; }
        public string Filter { get; set; }
    }

    public class Dyn365Entity
    {
        public string LogicalName { get; set; }
        public List<Dyn365Attribute> Attributes { get; set; }
        private Guid? _id;
        public Guid? Id { get { return _id; } set { this.Add(string.Concat(LogicalName, "id"), CrmType.String, value.ToString()); _id = value.Value; } }

        public Dyn365Entity(string logicalName)
        {
            LogicalName = logicalName;
            Attributes = new List<Dyn365Attribute>();
        }

        public void Add(string attrName, CrmType type, string content)
        {
            this.Attributes.Add(new Dyn365Attribute()
            {
                Type = type,
                Content = content,
                AttributeName = attrName
            }); ;
        }

        public void Add(string attributeName, Dyn365EntityReference ef)
        {
            if (ef == null)
            {
                this.Attributes.Add(new Dyn365Attribute()
                {
                    Type = CrmType.String,
                    AttributeName = attributeName,
                    Content = null
                });
            }
            else
            {
                this.Attributes.Add(new Dyn365Attribute()
                {
                    Type = CrmType.Lookup,
                    LookupEntityName = ef.EntityName,
                    AttributeName = attributeName,
                    Content = ef.Id.ToString()
                });
            }
        }

        public void AddLookup(string attributeName, Guid id, string entityName, bool isMultiLookup = false)
        {
            this.Attributes.Add(new Dyn365Attribute()
            {
                Type = CrmType.Lookup,
                LookupEntityName = entityName,
                AttributeName = attributeName,
                IsMultiLookup = isMultiLookup,
                Content = id.ToString()
            });
        }
    }

    public class Dyn365Attribute
    {
        public string AttributeName { get; set; }
        public CrmType Type { get; set; }
        public string LookupSearchField { get; set; }
        public string LookupEntityName { get; set; }
        public bool IsMultiLookup { get; set; }
        public string ActivityPartyFrom_EntityName { get; set; }
        public string ActivityPartyFrom_EntityId { get; set; }
        public string ActivityPartyTo_EntityName { get; set; }
        public string ActivityPartyTo_EntityId { get; set; }
        public string Content { get; set; }
    }

    public class Dyn365EntityReference
    {
        public Guid Id { get; set; }
        public string EntityName { get; set; }

    }

    public enum CrmType
    {
        ActivityParty,
        Lookup,
        String,
        OptionSet,
        Int,
        Bool,
        Decimal,
        Money,
        DateTime
    }

    #endregion
}
