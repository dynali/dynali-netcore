using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Dynali
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
