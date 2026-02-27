import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-unauthorized',
  template: `
    <div style="display:flex;flex-direction:column;align-items:center;justify-content:center;min-height:100vh;background:#f0f2f5;font-family:'Segoe UI',sans-serif">
      <div style="text-align:center;background:white;padding:40px;border-radius:16px;box-shadow:0 4px 20px rgba(0,0,0,0.08)">
        <div style="font-size:4rem">🚫</div>
        <h2 style="color:#1e1b4b;margin:16px 0 8px">Access Denied</h2>
        <p style="color:#6b7280">You don't have permission to access this page.</p>
        <button (click)="goBack()" style="margin-top:20px;padding:10px 24px;background:linear-gradient(135deg,#667eea,#764ba2);color:white;border:none;border-radius:8px;cursor:pointer;font-weight:600">
          Go Back
        </button>
      </div>
    </div>
  `
})
export class UnauthorizedComponent {
  constructor(private router: Router) {}
  goBack(): void { window.history.back(); }
}
