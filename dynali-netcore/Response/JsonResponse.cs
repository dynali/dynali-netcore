using Newtonsoft.Json;

namespace Dynali.Response
{
    public class JsonResponse
    {
        public bool IsSuccessful => (this.Code == 200);

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        public static T Parse<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
