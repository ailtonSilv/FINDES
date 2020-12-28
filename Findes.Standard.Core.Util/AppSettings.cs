using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Findes.Standard.Core.Util
{
    public class AppSettings
    {
        public string Dyn365URL { get; set; }
        public string ADURL { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string grant_type { get; set; }
        public string token { get; set; }
        public string AzureBlobConnectionString { get; set; }
    }
}
