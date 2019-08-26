using Newtonsoft.Json;
using System.Collections.Generic;

namespace Dynali.Action
{
    public abstract class DynaliAction : IDynaliAction
    {
        /// <summary>
        /// Storage for internal values. Replaces separate attributes for easier conversion in ToJson method.
        /// </summary>
        protected Dictionary<string, dynamic> payload = new Dictionary<string, dynamic>();

        public string ApiReferenceName
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Returns action in a form of JSON compatible with Dynali's API requests format.
        /// </summary>
        /// <returns>string</returns>
        public string ToJson()
        {
            if (ApiReferenceName == null || ApiReferenceName.Length == 0)
            {
                throw new DynaliException(-982, "Invalid or missing apiReferenceName");
            }

            return JsonConvert.SerializeObject(new Dictionary<string, dynamic>()
            {
                ["action"] = ApiReferenceName,
                ["payload"] = payload
            });
        }

        /// <summary>
        /// Validates attributes required for the action and returns a List of errors. 
        /// Returns empty List if no errors found.
        /// </summary>
        /// <returns>string[]</returns>
        public virtual List<string> GetValidationErrors()
        {
            return new List<string>();
        }
    }
}
