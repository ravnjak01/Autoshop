
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MyAuthService } from '../../core/services/auth/my-auth.service';
import{ Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { MyAuthInfo } from '../../modules/shared/dtos/my-auth-info';
import { TranslocoModule, TranslocoService } from '@ngneat/transloco';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    TranslocoModule
 
  ]
})
export class LoginComponent {

  loginForm: FormGroup;
  submitted = false;
  loading = false;
  errorMessage = '';
  

  constructor(
    private formBuilder: FormBuilder,
    private authService: MyAuthService, 
    private router: Router ,
      private httpClient: HttpClient ,
      public translocoService:TranslocoService
  ) {
   
    this.loginForm = this.formBuilder.group({
      password: ['', Validators.required],
      username: ['', Validators.required]
    });
  }
  
  changeLanguage(lang: string) {
    this.translocoService.setActiveLang(lang);
  }
navigateToForgotPassword() {
this.router.navigate(['/forgot-password']);
}


  get f() { return this.loginForm.controls; }

 
  onSubmit() {
  this.submitted = true;
  this.errorMessage = '';

  if (this.loginForm.invalid) {
    return;
  }

  this.loading = true;

  const { username, password } = this.loginForm.value;


 this.authService.login({ username, password }).subscribe({
  next: (response) => {
    if (response && response.token) {
      localStorage.setItem('jwtToken', response.token);
      this.authService.checkAuth().subscribe({
        next: () => this.router.navigate(['/home']),
        error: () => {
          this.errorMessage = 'auth.login.errors.general_error';
          this.loading = false;
        }
      });
    }
  },
  error: (err) => {
    if (err.status === 401) {
      this.errorMessage = 'auth.login.errors.invalid_credentials'; 
    } else {
      this.errorMessage = 'auth.login.errors.general_error';
    }
    this.loading = false;
  }
});
}



  
}
