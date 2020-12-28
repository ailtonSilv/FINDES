using System;
using System.Collections.Generic;
using System.Text;

namespace Findes.Standard.Core.Util
{
    public class Requisicao
    {
        public int Pagina { get; set; }
        public int QtdePagina { get; set; }
        public DateTime DataModificacao { get; set; }
        public string Layout { get; set; }
    }
}
