import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { TicketService } from '../../../core/services/ticket.service';
import { DashboardData } from '../../../shared/models/models';

@Component({
  selector: 'app-developer-dashboard',
  templateUrl: './developer-dashboard.component.html',
  styleUrls: ['../manager/manager-dashboard.component.scss']
})
export class DeveloperDashboardComponent implements OnInit {
  dashboard?: DashboardData;
  loading = true;

  constructor(
    public authService: AuthService,
    private ticketService: TicketService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.ticketService.getDeveloperDashboard().subscribe({
      next: (data) => { this.dashboard = data; this.loading = false; },
      error: () => this.loading = false
    });
  }

  viewTicket(id: number): void { this.router.navigate(['/tickets', id]); }
  viewMyTickets(): void { this.router.navigate(['/tickets/my']); }
  logout(): void { this.authService.logout(); }

  get statusEntries() {
    return Object.entries(this.dashboard?.statusCounts ?? {}).map(([key, value]) => ({ key, value }));
  }
}
