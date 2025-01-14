using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Expenses.Models
{
    public class Categories
    {
        [Key]

        public int CategoryId {  get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "This field is required.")]
        public string Title { get; set; } = "Expense";


        [Column(TypeName = "nvarchar(5)")]
        public string Icon { get; set; } = "";


        [Column(TypeName = "nvarchar(10)")]
        public string ?Type { get; set; }


        [NotMapped]
        public string? TitleWithIcon 
        {
            get { return Icon + " " + Title; }
        }


    }
}
