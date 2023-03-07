using Newtonsoft.Json;

namespace API.Models
{
    public class SwishCallback
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        //[JsonProperty("payeePaymentReference")]
        //public string PayeePaymentReference { get; set; }

        //[JsonProperty("paymentReference")]
        //public string PaymentReference { get; set; }

        //[JsonProperty("callbackUrl")]
        //public string CallbackUrl { get; set; }

        //[JsonProperty("payerAlias")]
        //public string PayerAlias { get; set; }

        //[JsonProperty("payeeAlias")]
        //public string PayeeAlias { get; set; }

        //[JsonProperty("amount")]
        //public double Amount { get; set; }

        //[JsonProperty("currency")]
        //public string Currency { get; set; }

        //[JsonProperty("message")]
        //public string Message { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        //[JsonProperty("dateCreated")]
        //public DateTime DateCreated { get; set; }

        //[JsonProperty("datePaid")]
        //public DateTime DatePaid { get; set; }
    }
}
