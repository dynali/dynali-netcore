using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Dynali
{
    public class JsonResponse    
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
