using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Findes.WebApi
{
    public class JsonFieldMapping
    {
        public string AttributeName { get; set; }
        public string Type { get; set; }
        public string LookupSearchField { get; set; }
        public string LookupEntityName { get; set; }
        public bool IsMultiLookup { get; set; }
        public string ActivityPartyFrom_EntityName { get; set; }
        public string ActivityPartyFrom_EntityId { get; set; }
        public string ActivityPartyTo_EntityName { get; set; }
        public string ActivityPartyTo_EntityId { get; set; }
        public string Content { get; set; }
    }
}
