import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PasswordStrengthService {

  calculatePasswordStrength(password: string): number {
    let strength = 0;

    if (!password) return strength;

  
    if (password.length >= 8) strength++;

 
    if (/[a-z]/.test(password)) strength++;

  
    if (/[A-Z]/.test(password)) strength++;

 
    if (/[0-9]/.test(password)) strength++;

    // 5. Specijalni znakovi
    if (/[\W_]/.test(password)) strength++;

    return strength; 
  }
}
