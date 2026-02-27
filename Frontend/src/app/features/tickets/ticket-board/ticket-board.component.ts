import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { TicketService } from '../../../core/services/ticket.service';
import { Ticket } from '../../../shared/models/models';

interface KanbanColumn {
  title: string;
  status: string;
  color: string;
  tickets: Ticket[];
}

@Component({
  selector: 'app-ticket-board',
  templateUrl: './ticket-board.component.html',
  styleUrls: ['./ticket-board.component.scss']
})
export class TicketBoardComponent implements OnInit {
  columns: KanbanColumn[] = [
    { title: 'New', status: 'New', color: '#3b82f6', tickets: [] },
    { title: 'Assigned', status: 'Assigned', color: '#8b5cf6', tickets: [] },
    { title: 'In Progress', status: 'WIP', color: '#f59e0b', tickets: [] },
    { title: 'Resolved', status: 'Resolved', color: '#10b981', tickets: [] },
    { title: 'Testing', status: 'Testing', color: '#06b6d4', tickets: [] },
    { title: 'Closed', status: 'Closed', color: '#6b7280', tickets: [] },
  ];

  loading = true;

  constructor(
    public authService: AuthService,
    private ticketService: TicketService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const obs = this.authService.userRole === 'Manager'
      ? this.ticketService.getAllTickets()
      : this.ticketService.getMyTickets();

    obs.subscribe({
      next: (tickets) => {
        this.columns.forEach(col => {
          col.tickets = tickets.filter(t => t.status === col.status);
        });
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  viewTicket(id: number): void { this.router.navigate(['/tickets', id]); }
}
