using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Dynali.Action
{
    public class ChangePasswordAction : HostnameAction
    {
        public new string ApiReferenceName
        {
            get
            {
                return "changepassword";
            }
        }

        public string NewPassword
        {
            get
            {
                return payload.ContainsKey("newpassword") ? payload["newpassword"] : null;
            }
            set
            {
                payload["newpassword"] = value;
            }
        }

        public override List<string> GetValidationErrors()
        {
            List<string> validationErrors = base.GetValidationErrors();

            if (!Regex.IsMatch(NewPassword, "^[a-f0-9]{32}$", RegexOptions.IgnoreCase))
            {
                validationErrors.Add("Invalid or missing new password.");
            }

            return validationErrors;
        }
    }
}
