using Newtonsoft.Json;

namespace API.Models
{
    public class SwishStatus
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("instructionUUID")]
        public string InstructionUUID { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
