using Newtonsoft.Json;

namespace Dynali.Response
{
    public class StatusPayload
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }

        [JsonProperty("expiry_date")]
        public string ExpiryDate { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("last_update")]
        public string LastUpdate { get; set; }
    }

    public class StatusResponse : JsonResponse
    {
        [JsonProperty("data")]
        public StatusPayload StatusPayload { get; set; }
    }
}
