namespace NeoBankWebApp.Models.PaymentGateway
{
   

    public class PaymentInitiateRequest
    {
         
        public string Link { get; set; }
        public string Amount { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string MailId { get; set; }
        public string Description { get; set; }
        public string IpAddress { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

    }

    public class PaymentInitiateRequestwol
    {

        public string CustomerId { get; set; }
        public string Amount { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string MailId { get; set; }
        public string Description { get; set; }

    }

    public class PaymentInitiate
    {

        public string CustomerId { get; set; } 
        public string Amount { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string MailId { get; set; }
        public string Description { get; set; }
        public string IpAddress { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

    }
    public class LinkResponseFinal
    {
        public string TxnID { get; set; }
        public string Link { get; set; }

    }

}
