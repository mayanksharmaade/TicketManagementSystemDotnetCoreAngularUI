import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { TicketService } from '../../../core/services/ticket.service';
import { Ticket } from '../../../shared/models/models';

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.scss']
})
export class TicketListComponent implements OnInit {
  tickets: Ticket[] = [];
  filteredTickets: Ticket[] = [];
  loading = true;
  isMyTickets = false;
  filterStatus = '';
  filterPriority = '';
  searchText = '';

  statuses = ['New', 'Assigned', 'WIP', 'Resolved', 'Testing', 'Closed', 'Rejected'];
  priorities = ['Low', 'Medium', 'High', 'Critical'];

  constructor(
    public authService: AuthService,
    private ticketService: TicketService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.isMyTickets = this.route.snapshot.url[0]?.path === 'my';
    this.loadTickets();
  }

  loadTickets(): void {
    const obs = this.isMyTickets
      ? this.ticketService.getMyTickets()
      : this.ticketService.getAllTickets();

    obs.subscribe({
      next: (data) => { this.tickets = data; this.applyFilters(); this.loading = false; },
      error: () => this.loading = false
    });
  }

  applyFilters(): void {
    this.filteredTickets = this.tickets.filter(t => {
      const matchStatus = !this.filterStatus || t.status === this.filterStatus;
      const matchPriority = !this.filterPriority || t.priority === this.filterPriority;
      const matchSearch = !this.searchText || t.title.toLowerCase().includes(this.searchText.toLowerCase());
      return matchStatus && matchPriority && matchSearch;
    });
  }

  viewTicket(id: number): void { this.router.navigate(['/tickets', id]); }
  createTicket(): void { this.router.navigate(['/tickets/create']); }

  get isManager(): boolean { return this.authService.userRole === 'Manager'; }
}
