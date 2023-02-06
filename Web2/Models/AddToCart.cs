using Newtonsoft.Json;

namespace API.Models
{
    public class AddToCart
    {
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
