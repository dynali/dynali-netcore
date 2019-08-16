using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dynali
{
    public class DynaliClient
    {
        const string EndpointLive = "https://api.dynali.net/nice/";
        const string EndpointDebug = "https://debug.dynali.net/nice/";

        static protected string GetMd5Hash(string input)
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

        static protected string Execute(Dictionary<string, dynamic> postdata, Boolean useDevEnv = false)
        {
            //prepare the request
            WebRequest request = WebRequest.CreateHttp(useDevEnv ? DynaliClient.EndpointDebug : DynaliClient.EndpointLive);
            request.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            request.Method = "POST";
            byte[] contentsBytes = UTF8Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postdata));
            request.GetRequestStream().Write(contentsBytes, 0, contentsBytes.Length);

            //get the response
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string responseString = reader.ReadToEnd();
            reader.Close();

            //return it
            return responseString;
        }

        async static protected Task<string> ExecuteAsync(Dictionary<string, dynamic> postdata, Boolean useDevEnv = false)
        {
            //prepare the request
            WebRequest request = WebRequest.CreateHttp(useDevEnv ? DynaliClient.EndpointDebug : DynaliClient.EndpointLive);
            request.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            request.Method = "POST";
            byte[] contentsBytes = UTF8Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postdata));
            (await request.GetRequestStreamAsync()).Write(contentsBytes, 0, contentsBytes.Length);

            //get the response
            StreamReader reader = new StreamReader((await request.GetResponseAsync()).GetResponseStream());
            string responseString = reader.ReadToEnd();
            reader.Close();

            //return it
            return responseString;
        }

        /// <summary>
        /// Returns client's IP as detected by Dynali.
        /// </summary>
        /// <returns>string, IP Address</returns>        
        static public string MyIp()
        {
            Dictionary<string, dynamic> body = new Dictionary<string, dynamic>() { ["action"] = "myip" };
            MyIpResponse response = JsonConvert.DeserializeObject<MyIpResponse>(Execute(body));

            if (response.Code == 200)
            {
                return response.Data.Ip;
            }

            throw new DynaliException(response.Code, response.Message);
        }

        /// <summary>
        /// Receives hostname's status.
        /// </summary>
        /// <param name="hostname">hostname</param>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>DynaliStatus; entity which represents hostname's status.</returns>
        static public DynaliStatus Status(string hostname, string username, string password, Boolean useDevEnv = false)
        {
            if (hostname.Length == 0)
            {
                throw new ArgumentException("Invalid or missing hostname.");
            }

            if (username.Length == 0)
            {
                throw new ArgumentException("Invalid or missing username.");
            }

            if (password.Length == 0)
            {
                throw new ArgumentException("Invalid or missing password.");
            }

            Dictionary<string, dynamic> body = new Dictionary<string, dynamic>()
            {
                ["action"] = "status",
                ["payload"] =
                {
                    ["hostname"] = hostname.ToLower(),
                    ["username"] = username,
                    ["password"] = GetMd5Hash(password).ToLower()
                }
            };

            StatusResponse response = JsonConvert.DeserializeObject<StatusResponse>(Execute(body, useDevEnv));

            if (response.Code == 200)
            {
                return new DynaliStatus(hostname, response.StatusPayload.Ip, response.StatusPayload.Status, response.StatusPayload.StatusMessage, DateTime.Parse(response.StatusPayload.ExpiryDate), DateTime.Parse(response.StatusPayload.Created), DateTime.Parse(response.StatusPayload.LastUpdate), DateTime.Now);
            }

            throw new DynaliException(response.Code, response.Message);
        }

        /// <summary>
        /// Updates password assigned to hostname.
        /// </summary>
        /// <param name="hostname">Hostname</param>
        /// <param name="username">Username assigned to hostname</param>
        /// <param name="password">Password assigned to hostname</param>
        /// <param name="newPassword">New password for hostname</param>
        /// <returns>boolean, true on success; throws Exception of failure</returns>
        static public bool ChangePassword(string hostname, string username, string password, string newPassword, Boolean useDevEnv = false)
        {
            if (hostname.Length == 0)
            {
                throw new ArgumentException("Invalid or missing hostname.");
            }

            if (username.Length == 0)
            {
                throw new ArgumentException("Invalid or missing username.");
            }

            if (password.Length == 0)
            {
                throw new ArgumentException("Invalid or missing password.");
            }

            if (newPassword.Length == 0)
            {
                throw new ArgumentException("Invalid or missing new password.");
            }

            Dictionary<string, dynamic> body = new Dictionary<string, dynamic>()
            {
                ["action"] = "changepassword",
                ["payload"] = {
                    ["username"] = username,
                    ["password"] = GetMd5Hash(password).ToLower(),
                    ["hostname"] = hostname.ToLower(),
                    ["newpassword"] = GetMd5Hash(newPassword).ToLower()
                }
            };

            JsonResponse response = JsonConvert.DeserializeObject<JsonResponse>(Execute(body, useDevEnv));
            if (response.Code == 200)
            {
                return true;
            }

            throw new DynaliException(response.Code, response.Message);
        }

        async static public Task<Boolean> ChangePasswordAsync(string hostname, string username, string password, string newPassword, Boolean useDevEnv = false)
        {
            if (hostname.Length == 0)
            {
                throw new ArgumentException("Invalid or missing hostname.");
            }

            if (username.Length == 0)
            {
                throw new ArgumentException("Invalid or missing username.");
            }

            if (password.Length == 0)
            {
                throw new ArgumentException("Invalid or missing password.");
            }

            if (newPassword.Length == 0)
            {
                throw new ArgumentException("Invalid or missing new password.");
            }

            Dictionary<string, dynamic> body = new Dictionary<string, dynamic>()
            {
                ["action"] = "changepassword",
                ["payload"] = {
                    ["username"] = username,
                    ["password"] = GetMd5Hash(password).ToLower(),
                    ["hostname"] = hostname.ToLower(),
                    ["newpassword"] = GetMd5Hash(newPassword).ToLower()
                }
            };

            JsonResponse response = JsonConvert.DeserializeObject<JsonResponse>(await ExecuteAsync(body, useDevEnv));
            if (response.Code == 200)
            {
                return true;
            }

            throw new DynaliException(response.Code, response.Message);
        }

        /// <summary>
        /// Updates client's hostname with client's ip. Username and password are required. IP can be provided or automatically detected.
        /// </summary>
        /// <param name="hostname">hostname</param>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <param name="ip">Valid IPv4 Address. Use "auto" in order to automatically detect your ip.</param>
        /// <returns>true on success; boolean</returns>
        static public bool Update(string hostname, string username, string password, string ip = "auto", Boolean useDevEnv = false)
        {
            if (hostname.Length == 0)
            {
                throw new ArgumentException("Invalid or missing hostname.");
            }

            if (username.Length == 0)
            {
                throw new ArgumentException("Invalid or missing username.");
            }

            if (password.Length == 0)
            {
                throw new ArgumentException("Invalid or missing password.");
            }

            if (ip != "auto" && !ValidateIPv4(ip))
            {
                throw new ArgumentException("Invalid IP. Provided `" + ip + "`.");
            }

            Dictionary<string, dynamic> body = new Dictionary<string, dynamic>()
            {
                ["action"] = "update",
                ["payload"] = {
                    ["username"] = username,
                    ["password"] = GetMd5Hash(password).ToLower(),
                    ["hostname"] = hostname.ToLower(),
                    ["myip"] = ip
                }
            };

            JsonResponse response = JsonConvert.DeserializeObject<JsonResponse>(Execute(body, useDevEnv));
            if (response.Code == 200)
            {
                return true;
            }

            throw new DynaliException(response.Code, response.Message);
        }

        static protected bool ValidateIPv4(string ipString)
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

    }
}
