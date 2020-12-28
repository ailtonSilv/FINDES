using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Findes.Standard.Core.Util
{
    public class ErrorHelper
    {
        public static void CatchError(Exception ex, string additionalInfo = "") {
            AppendLogError("Data: " + DateTime.Now.ToString() + "-------------------------------------");

            var _exception = ex.Message;
            if (ex.InnerException != null) {
                _exception = GetRawException(ex);
            }
            if (_exception != null) AppendLogError(_exception);

            if (!string.IsNullOrEmpty(additionalInfo)) AppendLogError(additionalInfo);
            AppendLogError(string.Concat("Trace:", ex.StackTrace));
            AppendLogError(string.Concat("Source: ", ex.Source));
        }

        public static string GetRawException(Exception ex) {
            var _exception = ex.Message;
            var originalException = ex.InnerException.GetOriginalException();
            if (originalException != null)
                _exception = originalException.Message;
            return _exception;
        }

        public static void AppendLogError(string erro) {
            try {
                string appDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().CodeBase).Replace(@"file:\", "") + @"\";
                StreamWriter sw = File.AppendText(string.Concat(appDir, "logError.txt"));
                sw.WriteLine(erro);
                sw.Dispose();
            }
            catch { }
        }
    }
}
