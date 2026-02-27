export interface User {
  id: number;
  fullName: string;
  email: string;
  role: string;
}

export interface AuthResponse {
  token: string;
  user: User;
}

export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterDto {
  fullName: string;
  email: string;
  password: string;
  role: number;
}

export interface Ticket {
  id: number;
  title: string;
  description: string;
  status: string;
  statusId: number;
  priority: string;
  priorityId: number;
  createdBy?: User;
  assignedTo?: User;
  createdAt: string;
  updatedAt?: string;
  dueDate?: string;
  comments: Comment[];
  history: TicketHistory[];
}

export interface CreateTicketDto {
  title: string;
  description: string;
  priority: number;
  dueDate?: string;
  assignedToId?: number;
}

export interface UpdateStatusDto {
  status: number;
  note?: string;
}

export interface AssignTicketDto {
  assignedToId: number;
}

export interface AddCommentDto {
  content: string;
}

export interface Comment {
  id: number;
  content: string;
  authorName: string;
  createdAt: string;
}

export interface TicketHistory {
  fromStatus: string;
  toStatus: string;
  changedBy: string;
  note?: string;
  changedAt: string;
}

export interface DashboardData {
  statusCounts: { [key: string]: number };
  recentTickets: Ticket[];
  totalTickets: number;
  openTickets: number;
  closedTickets: number;
}

export enum TicketStatus {
  New = 1,
  Assigned = 2,
  WIP = 3,
  Resolved = 4,
  Testing = 5,
  Closed = 6,
  Rejected = 7
}

export enum TicketPriority {
  Low = 1,
  Medium = 2,
  High = 3,
  Critical = 4
}

export enum UserRole {
  Manager = 1,
  Developer = 2,
  Tester = 3
}
