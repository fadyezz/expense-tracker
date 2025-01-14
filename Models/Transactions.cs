
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expenses.Models
{
    public class Transactions
    {
        [Key]
        public int TransactionId { get; set; }

        [Range(1,int.MaxValue,ErrorMessage = "This field is required.")]
        public int CategoryId { get; set; }
        public required Categories Category { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "This field is required.")]
        public int Amount { get; set; }

        [Column(TypeName = "nvarchar(80)")]
        public string? Note { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        [NotMapped]
        public string? CategoryTitleWithIcon
        {
            get { return Category == null ? "" : Category.Icon + " " + Category.Title; }
        }

        [NotMapped]

        public string? FormattedAmount
        {
            get { return ((Category == null || Category.Type == "Expense") ? "- ": "+ ") + Amount.ToString("C0"); }
        }
       
    }
}
 