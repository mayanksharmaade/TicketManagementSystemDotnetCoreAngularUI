import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UnauthorizedComponent } from './components/unauthorized.component';

@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule],
  exports: [CommonModule, RouterModule]
})
export class SharedModule {}
