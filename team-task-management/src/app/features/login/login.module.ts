import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common'; // 👈 هذا ضروري
import { FormsModule } from '@angular/forms'; // لو تستخدم ngModel
import { LoginComponent } from './login-page/login.component';

@NgModule({
  declarations: [LoginComponent],
  imports: [
    CommonModule, // 👈 ضروري لـ *ngIf و *ngFor
    FormsModule   // 👈 ضروري لـ [(ngModel)]
  ],
  exports: [LoginComponent]
})
export class LoginModule {}
