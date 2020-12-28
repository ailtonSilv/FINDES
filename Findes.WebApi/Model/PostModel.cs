using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Findes.WebApi.Model
{
    public class PostModel
    {
        public int? pageSize { get; set; }
        public int? pageNumber { get; set; }
        public string dataModificacao { get; set; }
        public string layout { get; set; }
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
