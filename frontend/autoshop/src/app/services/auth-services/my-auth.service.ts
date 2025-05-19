import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MyAuthInfo } from './dto/my-auth-info';
import { LoginTokenDto } from './dto/login-token-dto';
import {MySignalRService} from '../signalr-services/my-signal-r.service';

@Injectable({ providedIn: 'root' })
export class MyAuthService {
  private apiUrl = 'http://localhost:7000/api/auth'; // URL API-ja
  getMyAuthInfo(): MyAuthInfo | null {
    return this.getLoginToken()?.myAuthInfo ?? null;
  }
  constructor(private httpClient: HttpClient, private signalRService: MySignalRService) {}

  
  login(credentials: { username: string, password: string }): Observable<any> 
  {
  return this.httpClient.post('http://localhost:7000/api/auth/login', credentials, 
    {withCredentials: true }
);
}

    forgotPassword(email: string) {
  return this.httpClient.post<{ message: string, resetToken?: string }>(
    '/api/auth/forgot-password',
    { email }
  );
}

  register(data: { email: string, username:string,password: string }): Observable<any> {
    return this.httpClient.post(`${this.apiUrl}/register`, data);
  }
  isLoggedIn(): boolean {
    return this.getMyAuthInfo() != null && this.getMyAuthInfo()!.isLoggedIn;
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
/*
  register(userData: any) {
    return this.httpClient.post('http://localhost:7000/api/auth/register', userData);
  }*/
}
