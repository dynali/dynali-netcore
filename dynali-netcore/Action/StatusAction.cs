using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Dynali.Action
{
    public class StatusAction : HostnameAction
    {
        public new string ApiReferenceName
        {
            get
            {
                return "status";
            }
        }
    }
}
