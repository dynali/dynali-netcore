using Newtonsoft.Json;

namespace Dynali.Response
{
    public class MyIpPayload
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }
    }

    public class MyIpResponse : JsonResponse
    {
        [JsonProperty("data")]
        public MyIpPayload Data { get; set; }
    }
}
