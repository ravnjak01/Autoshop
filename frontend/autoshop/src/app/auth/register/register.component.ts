
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { NgForm } from '@angular/forms';
import { MyAuthService }  from '../../core/services/auth/my-auth.service';
import{Router, RouterModule}from '@angular/router';
import { CommonModule } from '@angular/common';
import { PasswordStrengthService } from './services/password-strength.service';
import { last } from 'rxjs';
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
  passwordStrength= 0;
  constructor(private fb: FormBuilder,private authService: MyAuthService,
    private router: Router,private passwordStrengthService: PasswordStrengthService) {
    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/;
    this.registrationForm = this.fb.group({
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      username: ['', Validators.required],
      password: ['', [
        Validators.required,
         Validators.minLength(8),
          Validators.pattern(passwordRegex) ]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });

    this.registrationForm.get('password')?.valueChanges.subscribe(value => {
  this.registrationForm.get('confirmPassword')?.updateValueAndValidity()
   this.passwordStrength=this.passwordStrengthService.calculatePasswordStrength(value);

     console.log("Strength = ", this.passwordStrength); 
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
    firstName: formData.firstName,
    lastName: formData.lastName,
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


getStrengthLabel(strength: number): string {
  switch (strength) {
    case 0: return 'Very weak';
    case 1: return 'Weak';
    case 2: return 'Average';
    case 3: return 'Good';
    case 4: return 'Strong';
    case 5: return 'Very strong';
    default: return '';
  }
}

}