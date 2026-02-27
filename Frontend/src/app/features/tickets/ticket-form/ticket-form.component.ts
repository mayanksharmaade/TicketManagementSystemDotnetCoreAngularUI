import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { TicketService } from '../../../core/services/ticket.service';
import { User } from '../../../shared/models/models';

@Component({
  selector: 'app-ticket-form',
  templateUrl: './ticket-form.component.html',
  styleUrls: ['./ticket-form.component.scss']
})
export class TicketFormComponent implements OnInit {
  ticketForm: FormGroup;
  developers: User[] = [];
  loading = false;
  error = '';

  priorities = [
    { value: 1, label: 'Low' },
    { value: 2, label: 'Medium' },
    { value: 3, label: 'High' },
    { value: 4, label: 'Critical' }
  ];

  constructor(
    private fb: FormBuilder,
    private ticketService: TicketService,
    public authService: AuthService,
    private router: Router
  ) {
    this.ticketForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(5)]],
      description: ['', [Validators.required, Validators.minLength(10)]],
      priority: [2, Validators.required],
      dueDate: [''],
      assignedToId: [null]
    });
  }

  ngOnInit(): void {
    this.ticketService.getDevelopers().subscribe(d => this.developers = d);
  }

  onSubmit(): void {
    if (this.ticketForm.invalid) return;
    this.loading = true;

    const value = this.ticketForm.value;
    if (!value.assignedToId) delete value.assignedToId;
    if (!value.dueDate) delete value.dueDate;

    this.ticketService.createTicket(value).subscribe({
      next: (t) => this.router.navigate(['/tickets', t.id]),
      error: (err) => { this.error = err.error?.message || 'Failed to create ticket.'; this.loading = false; }
    });
  }

  cancel(): void { this.router.navigate(['/tickets']); }
}
