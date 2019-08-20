using System;
using System.Collections.Generic;
using System.Text;

namespace Dynali.Action
{
    public interface IDynaliAction
    {
        /// <summary>
        /// Action's reference to Dynali's API.
        /// </summary>
        string ApiReferenceName { get; }

        /// <summary>
        /// Returns action in a form of JSON compatible with Dynali's API requests format.
        /// </summary>
        /// <returns>string</returns>
        string ToJson();

        /// <summary>
        /// Validates attributes required for the action and returns an array of errors. 
        /// Returns empty array if no errors found.
        /// </summary>
        /// <returns>string[]</returns>
        List<string> GetValidationErrors();
    }
}
