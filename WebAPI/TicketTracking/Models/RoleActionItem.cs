using Shared.ViewModels;
using System;

namespace TicketTracking.Models
{
    public class RoleActionItem
    {
        public RoleActionItem()
        {

        }

        public RoleActionItem(TicketRole role, string action)
        {
            Id = new Guid();
            Role = role;
            Action = action;
        }

        public Guid Id { get; set; }
        public TicketRole Role { get; set; }
        public string Action { get; set; }
    }

    public enum TicketRole
    {
        QA = 1,
        RD = 2,
        PM = 3,
        Admin = 4,
    }
}
