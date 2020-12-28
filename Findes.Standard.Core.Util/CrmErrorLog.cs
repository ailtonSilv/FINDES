using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Findes.Standard.Core.Util
{
    public class CrmErrorLog
    {
        IOptions<AppSettings> AppSettings;
        Dyn365Helper helper;

        public CrmErrorLog(IOptions<AppSettings> appSetttings)
        {
            AppSettings = appSetttings;
            helper = new Dyn365Helper(appSetttings);
            helper.Authenticate().Wait();
        }

        public void Write(Exception ex, string UseCase, string From, string To, string CustomMessage = null)
        {
            if (string.IsNullOrEmpty(CustomMessage))
                CustomMessage = ex.Message;

            Write(CustomMessage, ex.ToString(), UseCase, From, To);
        }

        public void Write(string message, string detail, string useCase, string from, string to)
        {
            var e = new Dyn365Entity("copasa_errorlog");

            e.Add("subject", CrmType.String, message.Substring(0, 40));
            e.Add("copasa_details", CrmType.String, detail);
            e.Add("copasa_from", CrmType.String, from);
            e.Add("copasa_to", CrmType.String, to);
            e.Add("copasa_usecase", CrmType.String, useCase);

            helper.Create(e);
        }
    }
}
