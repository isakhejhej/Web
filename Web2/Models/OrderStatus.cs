using Newtonsoft.Json;

namespace API.Models
{
    public class OrderStatus
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("orderId")]
        public int OrderId { get; set; }
        //[JsonProperty("userId")]
        //public Guid UserId { get; set; }
        [JsonProperty("created")]
        public DateTime Created { get; set; }
        [JsonProperty("paid")]
        public Byte Paid { get; set; }
    }
}
