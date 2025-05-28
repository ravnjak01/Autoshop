import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { MyAuthInfo } from './dto/my-auth-info';
import { LoginTokenDto } from './dto/login-token-dto';
import {MySignalRService} from '../signalr-services/my-signal-r.service';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
@Injectable({ providedIn: 'root' })
export class MyAuthService {
  private apiUrl = 'http://localhost:7000/api/auth'; // URL API-ja
    private isLoggedInSubject = new BehaviorSubject<boolean>(this.hasStoredLogin());
  isLoggedIn$ = this.isLoggedInSubject.asObservable();
  getMyAuthInfo(): MyAuthInfo | null {
    return this.getLoginToken()?.myAuthInfo ?? null;
  }
  constructor(private httpClient: HttpClient, private signalRService: MySignalRService,private router :Router) {}
 
  private hasStoredLogin(): boolean {
    return (
    !!localStorage.getItem('userName') &&
    !!localStorage.getItem('userRole')
  );
  }
  
  login(credentials: { username: string, password: string }): Observable<any> 
  {
  return this.httpClient.post('http://localhost:7000/api/auth/login', credentials, 
    {withCredentials: true })
    .pipe(
      tap((response: any) => {
      localStorage.setItem('userRole',response.role);
      localStorage.setItem('userName',response.username);

      this.isLoggedInSubject.next(true);
      })
      );
}
logout(): void {
  this.httpClient.post('http://localhost:7000/api/auth/logout', {},{withCredentials: true })
  .subscribe({

    next: () => {
      localStorage.removeItem('userRole');
      localStorage.removeItem('userName');
      localStorage.removeItem('my-auth-token');
      this.router.navigate(['/login']);
      this.isLoggedInSubject.next(false);
    },
    error:(err)=>{
      console.error('Logout failed', err);

      localStorage.removeItem('userRole');
      localStorage.removeItem('userName');
       this.isLoggedInSubject.next(false);
      this.router.navigate(['/login']);
    }
  });
}
    forgotPassword(email: string) {
  return this.httpClient.post<{ message: string, resetToken?: string }>(
    `${this.apiUrl}/forgot-password`,
    { email }
  );
}
resetPassword(data: { email: string, token: string, newPassword: string }) {
  return this.httpClient.post('http://localhost:7000/api/auth/reset-password', data);
}

  register(data: { email: string, username:string,password: string }): Observable<any> {
    return this.httpClient.post(`${this.apiUrl}/register`, data);
  }
  isLoggedIn(): boolean {
      return this.hasStoredLogin();
  }

  isAdmin(): boolean {
    return this.getMyAuthInfo()?.isAdmin ?? false;
  }

  isManager(): boolean {
    return this.getMyAuthInfo()?.isManager ?? false;
  }
  getUserData(): Observable<any> {
    return this.httpClient.get(`${this.apiUrl}/user`);
  }
  setLoggedInUser(x: LoginTokenDto | null) {
    if (x == null) {
      window.localStorage.setItem('my-auth-token', '');
      this.signalRService.stopConnection(); // Zaustavljanje SignalR konekcije nakon odjave
    } else {
      window.localStorage.setItem('my-auth-token', JSON.stringify(x));
      this.signalRService.startConnection(); // Pokretanje SignalR konekcije nakon prijave
    }
  }

  getLoginToken(): LoginTokenDto | null {
    let tokenString = window.localStorage.getItem('my-auth-token') ?? '';
    try {
      return JSON.parse(tokenString);
    } catch (e) {
      return null;
    }
  }
 


}
