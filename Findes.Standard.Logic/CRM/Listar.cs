using Findes.Standard.Core.Util;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Findes.Standard.Logic.CRM
{
    public class Listar
    {
        IOptions<AppSettings> AppSettings;

        public Listar(IOptions<AppSettings> appSettings)
        {
            AppSettings = appSettings;
        }

        public JObject ListarObjetos(JObject jsonParam)
        {
            var dyn365H = new Dyn365Helper(this.AppSettings);
            dyn365H.Authenticate().Wait();

            var webApiURI = this.AppSettings.Value.Dyn365URL + "/api/data/v9.1/findes_ActionExtratorCRM";

            var response = dyn365H.SendCrmRequestAsync(webApiURI, jsonParam).Result;

            var message = JObject.Parse(response.Content.ReadAsStringAsync().Result).SelectToken("Message");
            var content = JObject.Parse(response.Content.ReadAsStringAsync().Result).SelectToken("Result");

            if (message != null && message.ToString() != "Sucesso.")
            {
                Retorno ret = new Retorno
                {
                    mensagem = message.ToString(),
                    sucesso = false
                };
                string json = JsonConvert.SerializeObject(ret, Formatting.Indented);
                return JObject.Parse(json);
            }
            if (content != null)
            {
                return JObject.Parse(content.ToString());
            }

            return null;
        }

        [DataContract]
        public class Retorno
        {
            [DataMember]
            public string mensagem { get; set; }
            [DataMember]
            public bool sucesso { get; set; }
        }
    }
}
