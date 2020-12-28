using Findes.Standard.Core.Util;
using Findes.WebApi.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Findes.WebApi.Util
{
    public class Process
    {
        IOptions<AppSettings> AppSettings;
        public Process(IOptions<AppSettings> appSetttings)
        {
            AppSettings = appSetttings;
        }
        public JObject ProcessPostModel(PostModel value, string layout)
        {
            if (layout != null &&
                value.pageNumber != null &&
                value.pageSize != null &&
                value.dataModificacao != null)
            {
                string[] strData = value.dataModificacao.Split('/');
                string dia = strData[0];
                string mes = strData[1];
                string ano = strData[2];
                int pagina = Int32.Parse(value.pageNumber.ToString()) + 1;

                Requisicao requisicao = new Requisicao
                {
                    QtdePagina = Int32.Parse(value.pageSize.ToString()),
                    Pagina = pagina,
                    Layout = layout
                    //Layout = value.layout.ToString()
                };
                requisicao.DataModificacao = DateTime.Parse(ano + '/' + mes + '/' + dia);

                var jsonApp = JObject.FromObject(requisicao);
                var logic = new Standard.Logic.CRM.Listar(AppSettings);
                var result = logic.ListarObjetos(jsonApp);

                return result;
            }
            else
            {
                JObject jRetorno = JObject.Parse(@"{ 'mensagem': 'Modelo incorreto', 'sucesso': false }");
                return jRetorno;
            }
        }
    }
    
}
