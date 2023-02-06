using Newtonsoft.Json;

namespace API.Models
{
    public class StatusCode
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
