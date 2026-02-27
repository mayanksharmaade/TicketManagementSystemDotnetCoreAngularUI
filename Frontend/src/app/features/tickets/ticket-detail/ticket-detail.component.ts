import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { TicketService } from '../../../core/services/ticket.service';
import { Ticket, TicketStatus, User } from '../../../shared/models/models';

@Component({
  selector: 'app-ticket-detail',
  templateUrl: './ticket-detail.component.html',
  styleUrls: ['./ticket-detail.component.scss']
})
export class TicketDetailComponent implements OnInit {
  ticket?: Ticket;
  loading = true;
  commentForm: FormGroup;
  developers: User[] = [];
  submitting = false;

  availableTransitions: { label: string; status: number; color: string }[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    public authService: AuthService,
    private ticketService: TicketService
  ) {
    this.commentForm = this.fb.group({ content: ['', Validators.required] });
  }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.ticketService.getTicketById(id).subscribe({
      next: (t) => { this.ticket = t; this.setTransitions(); this.loading = false; },
      error: () => this.loading = false
    });

    if (this.isManager) {
      this.ticketService.getDevelopers().subscribe(d => this.developers = d);
    }
  }

  setTransitions(): void {
    if (!this.ticket) return;
    const statusId = this.ticket.statusId;
    const role = this.authService.userRole;

    const allTransitions: { [key: string]: { label: string; status: number; color: string }[] } = {
      Manager: [
        { label: 'Assign', status: TicketStatus.Assigned, color: '#8b5cf6' },
        { label: 'Close', status: TicketStatus.Closed, color: '#6b7280' },
        { label: 'Reject', status: TicketStatus.Rejected, color: '#ef4444' },
      ],
      Developer: [
        { label: 'Start WIP', status: TicketStatus.WIP, color: '#f59e0b' },
        { label: 'Resolve', status: TicketStatus.Resolved, color: '#10b981' },
      ],
      Tester: [
        { label: 'Start Testing', status: TicketStatus.Testing, color: '#06b6d4' },
        { label: 'Close (Pass)', status: TicketStatus.Closed, color: '#6b7280' },
        { label: 'Reopen (Fail)', status: TicketStatus.Assigned, color: '#ef4444' },
      ]
    };

    this.availableTransitions = allTransitions[role] ?? [];
  }

  updateStatus(status: number, note?: string): void {
    if (!this.ticket) return;
    this.ticketService.updateStatus(this.ticket.id, { status, note }).subscribe({
      next: (t) => { this.ticket = t; this.setTransitions(); }
    });
  }

  assignTicket(developerId: number): void {
    if (!this.ticket) return;
    this.ticketService.assignTicket(this.ticket.id, { assignedToId: developerId }).subscribe({
      next: (t) => { this.ticket = t; this.setTransitions(); }
    });
  }

  addComment(): void {
    if (this.commentForm.invalid || !this.ticket) return;
    this.submitting = true;
    this.ticketService.addComment(this.ticket.id, this.commentForm.value).subscribe({
      next: () => {
        this.commentForm.reset();
        this.ticketService.getTicketById(this.ticket!.id).subscribe(t => { this.ticket = t; this.submitting = false; });
      },
      error: () => this.submitting = false
    });
  }

  goBack(): void { window.history.back(); }
  get isManager(): boolean { return this.authService.userRole === 'Manager'; }
}
