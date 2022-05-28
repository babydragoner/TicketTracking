﻿using System;

namespace TicketTracking.Models
{
    public class TicketItem
    {
        public void SetTicketItem(TicketDTO item)
        {
            Id = item.Id;
            Title = item.Title;
            Summary = item.Summary;
            Description = item.Description;
            TicketType = item.TicketType;
            Status = item.Status;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public TicketType TicketType { get; set; }
        public TicketStatus Status { get; set; }
        public string CreUser { get; set; }
        public DateTime CreDate { get; set; }
        public string UpdUser { get; set; }
        public DateTime? UpdDate { get; set; }
    }

    public class TicketDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public TicketType TicketType { get; set; }
        public TicketStatus Status { get; set; }
    }

    public enum TicketStatus
    {
        // 建立 Ticket
        Open = 1,
        // Ticket 完成
        Finish = 2,
    }
    public enum TicketType
    {
        // Function bug
        Bug = 1,
        // New Function
        New = 2,
        // TestCase
        TestCase = 3,
    }
}
