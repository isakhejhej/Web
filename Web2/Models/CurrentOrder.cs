using Newtonsoft.Json;

namespace API.Models
{
    public class CurrentOrder
    {
        [JsonProperty("order")]
        public IEnumerable<OrderProduct> Order { get; set; }
        [JsonProperty("totalPrice")]
        public int TotalPrice { get; set; }

    }
}
