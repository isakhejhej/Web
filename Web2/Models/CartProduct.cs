using Newtonsoft.Json;

namespace API.Models
{
    public class CartProduct
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("userId")]
        public Guid UserId { get; set; }
        [JsonProperty("productId")]
        public Guid ProductId { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
