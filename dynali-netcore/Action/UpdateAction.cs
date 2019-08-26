using System;
using System.Collections.Generic;
using System.Linq;

namespace Dynali.Action
{
    public class UpdateAction : HostnameAction
    {
        public new string ApiReferenceName
        {
            get
            {
                return "update";
            }
        }

        public string Ip
        {
            get
            {
                return payload.ContainsKey("ip") ? payload["ip"] : null;
            }
            set
            {
                payload["ip"] = value;
            }
        }

        /// <summary>
        /// Verifies if provided string is a valid IPv4 domain.
        /// </summary>
        /// <param name="ipString">IPv4 string; use "auto" for automatic detection.</param>
        /// <returns>boolean</returns>
        protected bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;
            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }

        public override List<string> GetValidationErrors()
        {
            List<string> validationErrors = base.GetValidationErrors();

            if (Ip != "auto" && !ValidateIPv4(Ip))
            {
                validationErrors.Add("Invalid IP. Provided `" + Ip + "`.");
            }

            return validationErrors;
        }

    }
}
