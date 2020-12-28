using System;

namespace Findes.Standard.Core.Util
{
    public static class ExceptionExtensions
    {
        public static Exception GetOriginalException(this Exception ex) {
            if (ex == null || ex.InnerException == null) return null;
            return ex.InnerException.GetOriginalException();
        }
    }
}
