
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Xrm.Sdk.Query;
using System.Net;

namespace ConsoleTest
{
    public class ConsoleHelper
    {
        protected static int line = 0;
        protected static int processedTotal = 0;
        public static void Log(string text)
        {
            line++;
            string msg = $"{line.ToString()} - {DateTime.Now.ToString("G")} => {text}";
            Console.WriteLine(msg);
            if (line == 1)
            {
                AppendLogError("====================================================");
            }
            AppendLogError(msg);
        }

        public static void AppendLogError(string erro)
        {
            try
            {
                string appDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().CodeBase).Replace(@"file:\", "") + @"\";
                StreamWriter sw = File.AppendText(string.Concat(appDir, "logError.txt"));
                sw.WriteLine(erro);
                sw.Dispose();
            }
            catch { }
        }

        protected static int lineCsv = 0;
        public static void AppendCSV(string text, string file)
        {
            try
            {
                string appDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().CodeBase).Replace(@"file:\", "") + @"\";
                //pFile.AppendText(string.Concat(appDir, file))
                StreamWriter sw = new StreamWriter(string.Concat(appDir, file), true, Encoding.GetEncoding("iso-8859-1"));
                //StreamWriter sw = File.AppendText(string.Concat(appDir, file));
                sw.WriteLine(text);
                sw.Dispose();
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        public static void GenerateCSVHeader(Dictionary<string, Tuple<string, string>> fields, string file)
        {
            try
            {
                var orderned = fields.OrderBy(x => x.Value);
                StringBuilder sb = new StringBuilder();
                foreach (var field in orderned)
                {
                    string fieldName = field.Value.Item1;
                    var fieldKey = field.Key;
                    if (fieldKey.IndexOf("(id)") > 0)
                    {
                        fieldKey = fieldKey.Replace("(id)", "");
                    }
                    sb.Append($"{fieldName}; ");
                }
                ConsoleHelper.AppendCSV(sb.ToString(), file);
            }
            catch (Exception ex)
            {
                ConsoleHelper.Log(ex.Message);
            }
        }

        protected static int origRow;
        protected static int origCol;

        public static void SetPosition()
        {
            origRow = Console.CursorTop;
            origCol = Console.CursorLeft;
        }

        public static void ShowProgress(string s)
        {
            Console.SetCursorPosition(0, origRow);
            Console.CursorVisible = false;
            if (s == "100,00 %")
            {
                Console.WriteLine(s, s.Length+1);

            }
            else
            {
                Console.Write(s, s.Length);
            }
            
        }
    }
}
