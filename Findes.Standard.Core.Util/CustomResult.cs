using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Findes.Standard.Core.Util
{
    public class CustomResult
    {
        private string _id;
        private int _status;
        private string _message;

        public string Id {
            get { return _id; }
            set { _id = value; }
        }

        public int Status {
            get { return _status; }
            set { _status = value; }
        }

        public string Message {
            get { return _message; }
            set { _message = value; }
        }
    }
}

