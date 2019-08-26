using Dynali.Action;
using Dynali.Entity;
using Dynali.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dynali
{
    public class DynaliClient
    {
        private const string EndpointLive = "https://api.dynali.net/nice/";
        private const string EndpointDebug = "https://debug.dynali.net/nice/";
        public enum DynaliEnvironmentTarget { DEBUG = 0, LIVE = 1 };
        public DynaliEnvironmentTarget DynaliEnvironment { get; set; } = DynaliEnvironmentTarget.LIVE;

        /// <summary>
        /// Prepares WebRequest object.
        /// </summary>
        /// <param name="dynaliEnvironment">Sele</param>
        /// <returns>WebRequest</returns>
        protected WebRequest PrepareWebRequest()
        {
            WebRequest request = WebRequest.CreateHttp(DynaliEnvironment == DynaliEnvironmentTarget.DEBUG ? DynaliClient.EndpointDebug : DynaliClient.EndpointLive);
            request.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            request.Method = "POST";
            return request;
        }

        /// <summary>
        /// Validates action's parameters.
        /// Throws exception of failure, does nothing on success.
        /// </summary>
        /// <param name="action">IDynaliAction, request's action</param>
        protected void ValidateAction(IDynaliAction action)
        {
            List<string> validationErrors = action.GetValidationErrors();
            if (validationErrors.Count > 0)
            {
                throw new ArgumentException("Invalid request parameters: " + string.Join(", ", validationErrors));
            }
        }

        /// <summary>
        /// Reads response from Stream into a string.
        /// </summary>
        /// <param name="responseStream">Stream, input stream</param>
        /// <returns>string, response</returns>
        protected string ReadResponse(Stream responseStream)
        {
            StreamReader reader = new StreamReader(responseStream);
            string responseString = reader.ReadToEnd();
            reader.Close();
            return responseString;
        }

        /// <summary>
        /// Calls the Dynali's webservice, sends action's parameters as json.
        /// </summary>
        /// <param name="action">IDynaliAction, request's action</param>
        /// <returns>string, JSON response</returns>
        protected string Call(IDynaliAction action)
        {
            ValidateAction(action);
            byte[] contentsBytes = UTF8Encoding.UTF8.GetBytes(action.ToJson());
            WebRequest request = PrepareWebRequest();
            request.GetRequestStream().Write(contentsBytes, 0, contentsBytes.Length);

            Stream responseStream = request.GetResponse().GetResponseStream();
            return ReadResponse(responseStream);
        }

        /// <summary>
        /// Calls the Dynali's webservice asynchronously, sends action's parameters as json.
        /// </summary>
        /// <param name="action">IDynaliAction, request's action</param>
        /// <returns>string, JSON response</returns>
        protected async Task<string> CallAsync(IDynaliAction action)
        {
            ValidateAction(action);
            byte[] contentsBytes = UTF8Encoding.UTF8.GetBytes(action.ToJson());
            WebRequest request = PrepareWebRequest();
            (await request.GetRequestStreamAsync()).Write(contentsBytes, 0, contentsBytes.Length);

            Stream responseStream = (await request.GetResponseAsync()).GetResponseStream();
            return ReadResponse(responseStream);
        }

        /// <summary>
        /// Returns client's IP as detected by Dynali.
        /// </summary>
        /// <returns>string, IP Address</returns>        
        public string RetrieveMyIp()
        {
            MyIpResponse response = ExecuteAction<MyIpResponse>(new MyIpAction());
            if (response.IsSuccessful)
            {
                return response.Data.Ip;
            }
            throw new DynaliException(response.Code, response.Message);
        }

        /// <summary>
        /// Returns client's IP as detected by Dynali.
        /// </summary>
        /// <returns>string, IP Address</returns>        
        public async Task<string> RetrieveMyIpAsync()
        {
            MyIpResponse response = JsonResponse.Parse<MyIpResponse>(await CallAsync(new MyIpAction()));
            if (response.IsSuccessful)
            {
                return response.Data.Ip;
            }
            throw new DynaliException(response.Code, response.Message);
        }

        /// <summary>
        /// Executes provided DynaliAction.
        /// </summary>
        /// <typeparam name="T">JsonResponse deriative</typeparam>
        /// <param name="action">Action details</param>
        /// <returns>JsonResponse deriative</returns>
        protected T ExecuteAction<T>(IDynaliAction action) where T : JsonResponse
        {
            return JsonResponse.Parse<T>(Call(action));
        }

        /// <summary>
        /// Executes provided DynaliAction.
        /// </summary>
        /// <typeparam name="T">JsonResponse deriative</typeparam>
        /// <param name="action">Action details</param>
        /// <returns>JsonResponse deriative</returns>
        protected async Task<T> ExecuteActionAsync<T>(IDynaliAction action) where T : JsonResponse
        {
            return JsonResponse.Parse<T>(await CallAsync(action));
        }

        /// <summary>
        /// Receives hostname's status.
        /// </summary>
        /// <param name="hostname">hostname</param>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>DynaliStatus; entity which represents hostname's status.</returns>
        public DynaliStatus RetrieveStatus(string hostname, string username, string password)
        {
            StatusResponse response = ExecuteAction<StatusResponse>(new StatusAction() { Hostname = hostname, Password = HostnameAction.GetMd5Hash(password).ToLower(), Username = username });
            if (response.IsSuccessful)
            {
                return new DynaliStatus(hostname, response);
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
        public async Task<DynaliStatus> RetrieveStatusAsync(string hostname, string username, string password)
        {
            StatusResponse response = await ExecuteActionAsync<StatusResponse>(new StatusAction() { Hostname = hostname, Password = HostnameAction.GetMd5Hash(password).ToLower(), Username = username });
            if (response.IsSuccessful)
            {
                return new DynaliStatus(hostname, response);
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
        public bool ChangePassword(string hostname, string username, string password, string newPassword)
        {
            JsonResponse response = ExecuteAction<JsonResponse>(new ChangePasswordAction() { Hostname = hostname, NewPassword = HostnameAction.GetMd5Hash(newPassword).ToLower(), Password = HostnameAction.GetMd5Hash(password).ToLower(), Username = username });
            if (response.IsSuccessful)
            {
                return true;
            }

            throw new DynaliException(response.Code, response.Message);
        }

        public async Task<Boolean> ChangePasswordAsync(string hostname, string username, string password, string newPassword)
        {
            JsonResponse response = await ExecuteActionAsync<JsonResponse>(new ChangePasswordAction() { Hostname = hostname, NewPassword = HostnameAction.GetMd5Hash(newPassword).ToLower(), Password = HostnameAction.GetMd5Hash(password).ToLower(), Username = username });
            if (response.IsSuccessful)
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
        public bool Update(string hostname, string username, string password, string ip = "auto")
        {
            JsonResponse response = ExecuteAction<JsonResponse>(new UpdateAction() { Hostname = hostname, Ip = ip, Password = HostnameAction.GetMd5Hash(password), Username = username });
            if (response.IsSuccessful)
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
        public async Task<bool> UpdateAsync(string hostname, string username, string password, string ip = "auto")
        {
            JsonResponse response = await ExecuteActionAsync<JsonResponse>(new UpdateAction() { Hostname = hostname, Ip = ip, Password = HostnameAction.GetMd5Hash(password), Username = username });
            if (response.IsSuccessful)
            {
                return true;
            }

            throw new DynaliException(response.Code, response.Message);
        }

    }
}
