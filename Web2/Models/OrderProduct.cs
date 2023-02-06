using Newtonsoft.Json;

namespace API.Models
{
    public class OrderProduct
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("price")]
        public int Price { get; set; }
    }
}
