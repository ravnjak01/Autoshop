
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MyAuthService } from '../../core/services/auth/my-auth.service';
import{ Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { MyAuthInfo } from '../../modules/shared/dtos/my-auth-info';
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
      private httpClient: HttpClient 
  ) {
   
    this.loginForm = this.formBuilder.group({
//      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      username: ['', Validators.required]
    });
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
      
      this.authService.checkAuth().subscribe(() => {
         this.httpClient.get<MyAuthInfo>('http://localhost:7000/api/user/me').subscribe({
           next: (user) => {
             localStorage.setItem('userRoles', JSON.stringify(user.roles));
             this.router.navigate(['/home']);
           }
         });
      });
    }
  }
});
}

  
}
