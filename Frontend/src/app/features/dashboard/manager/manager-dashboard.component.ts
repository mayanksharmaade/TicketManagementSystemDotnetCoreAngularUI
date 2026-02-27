import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { TicketService } from '../../../core/services/ticket.service';
import { DashboardData, Ticket } from '../../../shared/models/models';

@Component({
  selector: 'app-manager-dashboard',
  templateUrl: './manager-dashboard.component.html',
  styleUrls: ['./manager-dashboard.component.scss']
})
export class ManagerDashboardComponent implements OnInit {
  dashboard?: DashboardData;
  loading = true;

  statusColors: { [key: string]: string } = {
    New: '#3b82f6',
    Assigned: '#8b5cf6',
    WIP: '#f59e0b',
    Resolved: '#10b981',
    Testing: '#06b6d4',
    Closed: '#6b7280',
    Rejected: '#ef4444'
  };

  constructor(
    public authService: AuthService,
    private ticketService: TicketService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.ticketService.getManagerDashboard().subscribe({
      next: (data) => { this.dashboard = data; this.loading = false; },
      error: () => this.loading = false
    });
  }

  get statusEntries(): { key: string; value: number; color: string }[] {
    return Object.entries(this.dashboard?.statusCounts ?? {}).map(([key, value]) => ({
      key, value, color: this.statusColors[key] ?? '#6b7280'
    }));
  }

  createTicket(): void { this.router.navigate(['/tickets/create']); }
  viewAllTickets(): void { this.router.navigate(['/tickets']); }
  viewTicket(id: number): void { this.router.navigate(['/tickets', id]); }
  logout(): void { this.authService.logout(); }
}
