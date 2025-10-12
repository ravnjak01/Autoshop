import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MyAuthService } from '../../../core/services/auth/my-auth.service';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css'],
  standalone:true,
  imports: [ReactiveFormsModule,CommonModule]
})
export class ResetPasswordComponent implements OnInit {
  resetForm!: FormGroup;
  token: string = '';
  email: string = '';
  errorMessage: string = '';
  successMessage: string = '';
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private authService: MyAuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.token = this.route.snapshot.queryParamMap.get('token') || '';
    this.email = this.route.snapshot.queryParamMap.get('email') || '';
const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/;

    this.resetForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(8)],
       Validators.pattern(passwordRegex) ],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordsMatchValidator });
  }

  passwordsMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirm = form.get('confirmPassword')?.value;
    return password === confirm ? null : { passwordMismatch: true };
  }

  onSubmit(): void {
    if (this.resetForm.invalid) {
      return;
    }

    const { password } = this.resetForm.value;

    this.authService.resetPassword({
      email: this.email,
      token: this.token,
      newPassword: password
    }).subscribe({
      next: () => {
        this.successMessage = 'Lozinka je uspješno resetovana.';
        setTimeout(() => this.router.navigate(['/login']), 3000);
      },
      error: (err) => {
        this.errorMessage = 'Greška pri resetovanju lozinke. Pokušajte ponovo.';
        console.error(err);
      }
    });
  }
}
