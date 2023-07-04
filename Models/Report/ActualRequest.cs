namespace NeoBankWebApp.Models.Report
{
    public class ActualRequest
    {
        public string SelectedReport { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

    }

    public class ConvertedRequest
    {
        public string CustomerId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

    }

    public class ReportResponse
    {
        public string CustomerId { get; set; }
        public string Reference_Id { get; set; }
        public string BeneMobileNumber { get; set; }
        public string Amount { get; set; }
        public string MailId { get; set; }
        public string TransactionStatus { get; set; }
        public string CreatedTime { get; set; }
    }
}
