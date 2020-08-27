using TestCaseLegiosoft.Models.Enums;

namespace TestCaseLegiosoft.Models
{
    public class TransactionModel
    {
        public int TransactionId { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public TransactionType TransactionType { get; set; }
        public string ClientName { get; set; }
        public decimal Amount { get; set; }
    }
}
