
export interface TicketItem {
    id: string;
    name: string;
    ticketType: number;
    startDate: string;
    endDate: string;
    creUser: string;
    creDate: string;
    status: number;
    priority: string;
    assignUser: string;
  }

  export interface TicketsQueryResponse {
    activities: Array<TicketItem>;
  }