using Microsoft.EntityFrameworkCore;
using Shared.ViewModels;

namespace TicketTracking.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public void init()
        {
            RoleActionItems.Add(new RoleActionItem(TicketRole.QA, ActionType.Create));
            RoleActionItems.Add(new RoleActionItem(TicketRole.PM, ActionType.Create));
            RoleActionItems.Add(new RoleActionItem(TicketRole.Admin, ActionType.Create));
            RoleActionItems.Add(new RoleActionItem(TicketRole.QA, ActionType.Update));
            RoleActionItems.Add(new RoleActionItem(TicketRole.PM, ActionType.Update));
            RoleActionItems.Add(new RoleActionItem(TicketRole.Admin, ActionType.Update));
            RoleActionItems.Add(new RoleActionItem(TicketRole.RD, "complete"));
            this.SaveChanges();
        }

        public DbSet<TicketItem> TicketItems { get; set; } = null!;
        public DbSet<RoleActionItem> RoleActionItems { get; set; } = null!;
    }
}
