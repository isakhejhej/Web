namespace API.Models
{
    public class SwishCallback
    {
        public string Id { get; set; }
        public string payeePaymentReference { get; set; }
        public string paymentReference { get; set; }
        public string callbackUrl { get; set; }
        public string payerAlias { get; set; }
        public string payeeAlias { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string dateCreated { get; set; }
        public string datePaid { get; set; }
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
    }
}
