
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MyAuthService } from '../services/auth-services/my-auth.service';
import{ Router } from '@angular/router';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: false,
})
export class LoginComponent {

  loginForm: FormGroup;
  submitted = false;
  loading = false;
  errorMessage = '';
  

  constructor(
    private formBuilder: FormBuilder,
    private authService: MyAuthService, // Dodano korištenje servisa
    private router: Router // Dodano korištenje routera
  ) {
    // Inicijalizacija forme sa validacijama
    this.loginForm = this.formBuilder.group({
//      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      username: ['', Validators.required]
    });
  }
navigateToForgotPassword() {
this.router.navigate(['/forgot-password']);
}
  // Getter za olakšano pristupanje formama u template-u
  get f() { return this.loginForm.controls; }

  // Funkcija za slanje forme
  onSubmit() {
    this.submitted = true;
    this.errorMessage = '';

    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;

    const { email, password, username } = this.loginForm.value;

    this.authService.login({  username,password }).subscribe({
      next: (res) => {
        this.loading = false;
     
         this.router.navigate(['/home']);
      },
      error: (err:any) => {
        if(err.status===401 && err.error?.message ) {
          this.errorMessage = err.error.message;
        }
        else{
          this.errorMessage = 'Greška pri loginu';
        }
        this.loading = false;
      
      }
    });
  }
  
}
