
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { NgForm } from '@angular/forms';
import { MyAuthService } from '../services/auth-services/my-auth.service';
import{Router}from '@angular/router';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  standalone:false
})
export class RegisterComponent {
  registrationForm: FormGroup;

  constructor(private fb: FormBuilder,private authService: MyAuthService,
    private router: Router) {
    
    this.registrationForm = this.fb.group({
        fullname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;

    if (password !== confirmPassword) {
      form.get('confirmPassword')?.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    } else {
      return null;
    }
  }

  onSubmit() {
    if (this.registrationForm.valid) {
   const formData=this.registrationForm.value;

   const registrationData = {
    fullname: formData.fullname,
  email:formData.email, 
  username:formData.username,   
  password:formData.password,
  };

  this.authService.register(registrationData).subscribe({
    next:()=>{
    alert('Registration successful');
    this.router.navigate(['/login']);
    },
    error: (error) => {
      console.error('Registration failed', error);
      alert('Registration failed');
    }
  });
    }
  }
}