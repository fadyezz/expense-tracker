using Microsoft.EntityFrameworkCore;

namespace Expenses.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(
            DbContextOptions options ) : base(options) { } 
            
 
        public DbSet<Transactions> Transactions {  get; set; }
        public DbSet<Categories> Categories { get; set; }



    }
}
