import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { TicketListComponent } from './ticket-list/ticket-list.component';
import { TicketDetailComponent } from './ticket-detail/ticket-detail.component';
import { TicketFormComponent } from './ticket-form/ticket-form.component';
import { TicketBoardComponent } from './ticket-board/ticket-board.component';
import { AuthGuard } from '../../core/guards/auth.guard';
import { SharedModule } from '../../shared/shared.module';

const routes: Routes = [
  { path: '', component: TicketListComponent, canActivate: [AuthGuard], data: { roles: ['Manager'] } },
  { path: 'my', component: TicketListComponent, canActivate: [AuthGuard] },
  { path: 'create', component: TicketFormComponent, canActivate: [AuthGuard], data: { roles: ['Manager'] } },
  { path: 'board', component: TicketBoardComponent, canActivate: [AuthGuard] },
  { path: ':id', component: TicketDetailComponent, canActivate: [AuthGuard] },
];

@NgModule({
  declarations: [TicketListComponent, TicketDetailComponent, TicketFormComponent, TicketBoardComponent],
  imports: [CommonModule, ReactiveFormsModule, FormsModule, SharedModule, RouterModule.forChild(routes)]
})
export class TicketsModule {}
