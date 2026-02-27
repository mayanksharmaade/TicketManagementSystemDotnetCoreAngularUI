import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AddCommentDto, AssignTicketDto, CreateTicketDto, DashboardData, Ticket, UpdateStatusDto, User } from '../../shared/models/models';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class TicketService {
  private readonly API = environment.apiUrl;

  constructor(private http: HttpClient) {}

  // Tickets
  getAllTickets(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(`${this.API}/tickets`);
  }

  getMyTickets(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(`${this.API}/tickets/my`);
  }

  getTicketById(id: number): Observable<Ticket> {
    return this.http.get<Ticket>(`${this.API}/tickets/${id}`);
  }

  createTicket(dto: CreateTicketDto): Observable<Ticket> {
    return this.http.post<Ticket>(`${this.API}/tickets`, dto);
  }

  updateStatus(id: number, dto: UpdateStatusDto): Observable<Ticket> {
    return this.http.patch<Ticket>(`${this.API}/tickets/${id}/status`, dto);
  }

  assignTicket(id: number, dto: AssignTicketDto): Observable<Ticket> {
    return this.http.patch<Ticket>(`${this.API}/tickets/${id}/assign`, dto);
  }

  addComment(id: number, dto: AddCommentDto): Observable<any> {
    return this.http.post(`${this.API}/tickets/${id}/comments`, dto);
  }

  // Dashboard
  getManagerDashboard(): Observable<DashboardData> {
    return this.http.get<DashboardData>(`${this.API}/dashboard/manager`);
  }

  getDeveloperDashboard(): Observable<DashboardData> {
    return this.http.get<DashboardData>(`${this.API}/dashboard/developer`);
  }

  getTesterDashboard(): Observable<DashboardData> {
    return this.http.get<DashboardData>(`${this.API}/dashboard/tester`);
  }

  // Users
  getDevelopers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.API}/users/developers`);
  }

  getTesters(): Observable<User[]> {
    return this.http.get<User[]>(`${this.API}/users/testers`);
  }

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.API}/users`);
  }
}
