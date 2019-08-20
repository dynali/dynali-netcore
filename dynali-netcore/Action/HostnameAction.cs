using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace Dynali.Action
{
    abstract public class HostnameAction : DynaliAction, IDynaliAction
    {
        public string Hostname
        {
            get {
                return payload.ContainsKey("hostname") ? payload["hostname"] : null;
            }
            set {
                payload["hostname"] = value;
            }
        }

        public string Username
        {
            get
            {
                return payload.ContainsKey("username") ? payload["username"] : null;
            }
            set
            {
                payload["username"] = value;
            }
        }

        public string Password
        {
            get
            {
                return payload.ContainsKey("password") ? payload["password"] : null;
            }
            set
            {
                payload["password"] = value;
            }
        }

        static public string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public override List<string> GetValidationErrors()
        {
            List<string> validationErrors = base.GetValidationErrors();

            if (!Regex.IsMatch(Hostname, "^([a-z0-9\\-]+\\.)+[a-z]+$", RegexOptions.IgnoreCase))
            {
                validationErrors.Add("Invalid or missing hostname.");
            }

            if (!Regex.IsMatch(Username, "[a-z0-9]{4,128}$", RegexOptions.IgnoreCase))
            {
                validationErrors.Add("Invalid or missing username.");
            }
               
            if (!Regex.IsMatch(Password, "^[a-f0-9]{32}$", RegexOptions.IgnoreCase))
            {
                validationErrors.Add("Invalid or missing password.");
            }

            return validationErrors;
        }
    }
}
