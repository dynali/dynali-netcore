using Dynali.Response;
using System;

namespace Dynali.Entity
{
    /// <summary>
    /// Entity which represents information about Dynali hostname's status.
    /// </summary>
    public class DynaliStatus : ICloneable
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
        public bool IsActive => (Status == 0);

        /// <summary>
        /// Informs if hostname is still active in Dynali.
        /// </summary>
        public bool IsExpired => (Status == 2);

        /// <summary>
        /// Informs if hostname is still active in Dynali.
        /// </summary>
        public bool IsBanned => (Status == 9);

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
            Hostname = hostname;
            Ip = ip;
            Status = status;
            StatusMessage = statusMessage;
            ExpiryDate = expiryDate;
            CreationDate = creationDate;
            LastUpdateDate = lastUpdateDate;
            StatusCheckDate = statusCheckDate;
        }

        public DynaliStatus(string hostname, StatusResponse statusResponse)
        {
            Hostname = hostname;
            Ip = statusResponse.StatusPayload.Ip;
            Status = statusResponse.StatusPayload.Status;
            StatusMessage = statusResponse.StatusPayload.StatusMessage;
            ExpiryDate = DateTime.Parse(statusResponse.StatusPayload.ExpiryDate);
            CreationDate = DateTime.Parse(statusResponse.StatusPayload.Created);
            LastUpdateDate = DateTime.Parse(statusResponse.StatusPayload.LastUpdate);
            StatusCheckDate = DateTime.Now;
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

        public object Clone()
        {
            DynaliStatus cloned = (DynaliStatus)MemberwiseClone();
            return cloned;
        }
    }
}
