using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findes.Integracao.CargaBatch.Util
{
    public class CsvReader
    {
        #region Attributes and Constructor
        public char Delimiter { get; set; }
        public bool HasHeader { get; set; }

        public CsvReader(char delimiter, bool hasHeader)
        {
            Delimiter = delimiter;
            HasHeader = hasHeader;
        }
        #endregion

        public Csv Parse(string[] strArr)
        {
            var csv = new Csv();
            
            if (HasHeader)
                strArr = strArr.Skip(1).ToArray();

            for (var i = 0; i < strArr.Length; ++i)
            {
                var dirtyRow = strArr.ElementAt(i).Split(Delimiter);

                var cleanRow = new List<string>();
                for (var j = 0; j < dirtyRow.Length; ++j)
                {
                    cleanRow.Add(dirtyRow[j].Trim().Replace("\0", ""));
                }

                csv.Rows.Add(cleanRow);
            }

            return csv;
        }

        public class Csv
        {
            public List<List<string>> Rows { get; set; }

            public Csv()
            {
                this.Rows = new List<List<string>>();
            }
        }
    }
}
