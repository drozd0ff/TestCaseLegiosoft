using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestCaseLegiosoft.Models.Enums;

namespace TestCaseLegiosoft.Models
{
    public class TransactionModel
    {
        [Key]
        public int TransactionId { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public TransactionType TransactionType { get; set; }
        public string ClientName { get; set; }
        [Column(TypeName = "decimal(13, 2)")]
        public decimal Amount { get; set; }
    }
}
