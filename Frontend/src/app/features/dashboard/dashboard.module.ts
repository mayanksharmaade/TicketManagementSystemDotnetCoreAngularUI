import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../../core/guards/auth.guard';
import { ManagerDashboardComponent } from './manager/manager-dashboard.component';
import { DeveloperDashboardComponent } from './developer/developer-dashboard.component';
import { TesterDashboardComponent } from './tester/tester-dashboard.component';
import { SharedModule } from '../../shared/shared.module';

const routes: Routes = [
  { path: 'manager', component: ManagerDashboardComponent, canActivate: [AuthGuard], data: { roles: ['Manager'] } },
  { path: 'developer', component: DeveloperDashboardComponent, canActivate: [AuthGuard], data: { roles: ['Developer'] } },
  { path: 'tester', component: TesterDashboardComponent, canActivate: [AuthGuard], data: { roles: ['Tester'] } },
  { path: '', redirectTo: 'manager', pathMatch: 'full' }
];

@NgModule({
  declarations: [ManagerDashboardComponent, DeveloperDashboardComponent, TesterDashboardComponent],
  imports: [CommonModule, SharedModule, RouterModule.forChild(routes)]
})
export class DashboardModule {}
