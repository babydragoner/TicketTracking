using Microsoft.EntityFrameworkCore;

namespace TicketTracking.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public DbSet<TicketItem> TicketItems { get; set; } = null!;
    }
}
