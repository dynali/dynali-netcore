using System;
using Dynali.Response;

namespace Dynali.Entity
{
    /// <summary>
    /// Entity which represents information about Dynali hostname's status.
    /// </summary>
    public class DynaliStatus
    {
        /// <summary>
        /// Numerical representation of the status as returned by Dynali.
        /// </summary> 
        public int Status { get; private set; }

        /// <summary>
        /// IP address status assigned to a hostname.
        /// </summary> 
        public string Ip { get; private set; }

        /// <summary>
        /// Textual representation of the status as returned by Dynali.
        /// </summary> 
        public string StatusMessage { get; private set; }

        /// <summary>
        /// Expiry date (may be in the future or in the past).
        /// </summary> 
        public DateTime ExpiryDate { get; private set; }

        /// <summary>
        /// Hostname.
        /// </summary> 
        public string Hostname { get; private set; }

        /// <summary>
        /// Date of the hostname's creation.
        /// </summary> 
        public DateTime CreationDate { get; private set; }

        /// <summary>
        /// Date of the last update.
        /// </summary> 
        public DateTime LastUpdateDate { get; private set; }

        /// <summary>
        /// Date when this status check was performed.
        /// </summary> 
        public DateTime StatusCheckDate { get; private set; }

        /// <summary>
        /// Informs if hostname is still active in Dynali.
        /// </summary>
        public bool IsActive => (this.Status == 0);

        /// <summary>
        /// Informs if hostname is still active in Dynali.
        /// </summary>
        public bool IsExpired => (this.Status == 2);

        /// <summary>
        /// Informs if hostname is still active in Dynali.
        /// </summary>
        public bool IsBanned => (this.Status == 9);

        /// <summary>
        /// Creates new instance of the hostname's status entity.
        /// </summary>
        /// <param name="hostname">Hostname</param>
        /// <param name="ip">IP address assigned to the hostname</param>
        /// <param name="status">Numerical status</param>
        /// <param name="statusMessage">Textual status</param>
        /// <param name="expiryDate">Expiry date</param>
        /// <param name="creationDate">Creation date</param>
        /// <param name="lastUpdateDate">Last update date</param>
        /// <param name="statusCheckDate">Status check date</param>
        public DynaliStatus(string hostname, string ip, int status, string statusMessage, DateTime expiryDate, DateTime creationDate, DateTime lastUpdateDate, DateTime statusCheckDate)
        {
            this.Hostname = hostname;
            this.Ip = ip;
            this.Status = status;
            this.StatusMessage = statusMessage;
            this.ExpiryDate = expiryDate;
            this.CreationDate = creationDate;
            this.LastUpdateDate = lastUpdateDate;
            this.StatusCheckDate = statusCheckDate;
        }

        public DynaliStatus(string hostname, StatusResponse statusResponse)
        {
            this.Hostname = hostname;
            this.Ip = statusResponse.StatusPayload.Ip;
            this.Status = statusResponse.StatusPayload.Status;
            this.StatusMessage = statusResponse.StatusPayload.StatusMessage;
            this.ExpiryDate = DateTime.Parse(statusResponse.StatusPayload.ExpiryDate);
            this.CreationDate = DateTime.Parse(statusResponse.StatusPayload.Created);
            this.LastUpdateDate = DateTime.Parse(statusResponse.StatusPayload.LastUpdate);
            this.StatusCheckDate = DateTime.Now;
        }

        public override string ToString()
        {
            return "Hostname: " + Hostname + "\r\n"
                + "Ip: " + Ip + "\r\n"
                + "Status: " + Status + "\r\n"
                + "Status message: " + StatusMessage + "\r\n"
                + "Expiry date: " + ExpiryDate.ToLongDateString() + "\r\n"
                + "Creation date: " + CreationDate.ToLongDateString() + "\r\n"
                + "Last update: " + LastUpdateDate.ToLongDateString() + "\r\n"
                + "Status check date: " + StatusCheckDate.ToLongDateString() + "\r\n";
        }
    }
}
