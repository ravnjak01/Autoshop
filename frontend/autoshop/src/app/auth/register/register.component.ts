
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { NgForm } from '@angular/forms';
import { MyAuthService }  from '../../core/services/auth/my-auth.service';
import{Router, RouterModule}from '@angular/router';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  standalone:true,
    imports: [
    ReactiveFormsModule,
    RouterModule,
    CommonModule
  ]
})
export class RegisterComponent {
  registrationForm: FormGroup;

  constructor(private fb: FormBuilder,private authService: MyAuthService,
    private router: Router) {
    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/;
    this.registrationForm = this.fb.group({
        fullname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      username: ['', Validators.required],
      password: ['', [
        Validators.required,
         Validators.minLength(8),
          Validators.pattern(passwordRegex) ]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });

    this.registrationForm.get('password')?.valueChanges.subscribe(() => {
  this.registrationForm.get('confirmPassword')?.updateValueAndValidity();
});

  }

  passwordMatchValidator(form: FormGroup) {
  const password = form.get('password')?.value;
  const confirmPassword = form.get('confirmPassword')?.value;

  if (password !== confirmPassword) {
    form.get('confirmPassword')?.setErrors({ passwordMismatch: true });
    return { passwordMismatch: true };
  } else {
   

    const errors = form.get('confirmPassword')?.errors;
    if (errors) {
      delete errors['passwordMismatch'];
      if (Object.keys(errors).length === 0) {
        form.get('confirmPassword')?.setErrors(null);
      } else {
        form.get('confirmPassword')?.setErrors(errors);
      }
    }
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