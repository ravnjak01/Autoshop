import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MyAuthService } from '../../core/services/auth/my-auth.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MySnackbarHelperService } from '../../modules/shared/snackbars/my-snackbar-helper.service';

@Component({
  selector: 'app-forgot-password',
 templateUrl: '../forgot-password/forgot-password.html', 
  styleUrls: ['../forgot-password/forgot-password.css'] ,
  standalone:true,
    imports: [
    ReactiveFormsModule,
    CommonModule,
    RouterModule 
  ]
})
export class ForgotPasswordComponent {
  forgotPasswordForm: FormGroup;
  submitted = false;

  constructor(private fb: FormBuilder,private authService: MyAuthService,
    private snackbar:MySnackbarHelperService
  ) {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  get f() {
    return this.forgotPasswordForm.controls;
  }

  onSubmit() {
    this.submitted = true;
    if (this.forgotPasswordForm.invalid) return;

    const email = this.forgotPasswordForm.value.email;
 this.authService.forgotPassword(email).subscribe({
    next: () => {
      this.snackbar.showMessage('Please check your email for further instructions')
    },
    error: (err: Error) => {
      this.snackbar.showMessage("There was an error");
    }
  });
  }
}
