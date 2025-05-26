import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MyAuthService } from '../services/auth-services/my-auth.service';

@Component({
  selector: 'app-forgot-password',
 templateUrl: '../forgot-password/forgot-password.html', 
  styleUrls: ['../forgot-password/forgot-password.css'] ,
  standalone:false
})
export class ForgotPasswordComponent {
  forgotPasswordForm: FormGroup;
  submitted = false;

  constructor(private fb: FormBuilder,private authService: MyAuthService) {
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
      alert('Link za reset lozinke je poslan na vašu email adresu.');
    },
    error: (err: any) => {
      console.error('Greška pri slanju email-a:', err);
      alert('Došlo je do greške. Pokušajte ponovo.');
    }
  });
  }
}
