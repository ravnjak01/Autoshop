import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, Observable, of, switchMap, tap } from 'rxjs';
import { MyAuthInfo } from '../../../modules/shared/dtos/my-auth-info';
import { LoginTokenDto } from '../../../modules/shared/dtos/login-token-dto';
import {MySignalRService} from '../../../core/services/signalr/my-signal-r.service';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';


@Injectable({ providedIn: 'root' })
export class MyAuthService {
  private apiUrl = 'http://localhost:7000/api/auth';
    private isLoggedInSubject = new BehaviorSubject<boolean>(this.hasStoredLogin());
  isLoggedIn$ = this.isLoggedInSubject.asObservable();


  getMyAuthInfo(): MyAuthInfo | null {
  const rolesString = localStorage.getItem('userRoles');
  const username = localStorage.getItem('userName');
 if (!username || !rolesString) return null;

  return {
    username,
    roles: JSON.parse(rolesString) as string[]
  };
}

  constructor(private httpClient: HttpClient, private signalRService: MySignalRService,private router :Router) {

    window.addEventListener('storage', (event) => {
    if (event.key === 'jwtToken' && !event.newValue) {
      this.logout();
    }
  });
  }
 
  private hasStoredLogin(): boolean {
    return   !!localStorage.getItem('jwtToken');
  }
login(credentials: { username: string; password: string }): Observable<any> {
  return this.httpClient
    .post<any>(`${this.apiUrl}/login`, credentials)
    .pipe(
      tap((response) => {
        if (response && response.token) {
          localStorage.setItem('jwtToken', response.token);
          
          if (response.roles) {
            localStorage.setItem('userRoles', JSON.stringify(response.roles));
          }
          
        } else {
        }
      })
    );
}
getToken(): string | null {
  return localStorage.getItem('jwtToken');
}


checkAuth(): Observable<boolean> {
  const token = localStorage.getItem('jwtToken');
  if(!token) return of(false);

  const headers=new HttpHeaders({
    'Authorization': `Bearer ${token}`,
  });

 return this.httpClient
      .get<{ username: string; roles: string[] }>('http://localhost:7000/api/user/me', { headers })
      .pipe(
        map((response) => {
          if (response && response.username) {
            this.isLoggedInSubject.next(true);
            return true;
          }
          return false;
        }),
        catchError(() => {
          this.isLoggedInSubject.next(false);
          return of(false);
        })
      );
}
  logout(): void {
  
      localStorage.clear();

      this.isLoggedInSubject.next(false);
      this.router.navigate(['/login']);
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
    return this.isLoggedInSubject.value;
  }

  isAdmin(): boolean {
  const rolesString = localStorage.getItem('userRoles');
  if (!rolesString) return false;

  const roles: string[] = JSON.parse(rolesString);
  return roles.includes('Admin');
}
isManager(): boolean {
  return localStorage.getItem('userRole') === 'Manager';
}
  getUserData(): Observable<any> {
    return this.httpClient.get(`${this.apiUrl}/user`);
  }
  


getCurrentUserId(): Observable<string | null> {

    const cachedUserId = localStorage.getItem('userId');
  if (cachedUserId) return of(cachedUserId);

  const token = this.getToken();
  if (!token) return of(null);

  const headers = new HttpHeaders({
    'Authorization': `Bearer ${token}`,
  });

  return this.httpClient
    .get<{ id: string; username: string; email: string; roles: string[] }>('http://localhost:7000/api/user/me', { headers })
       .pipe(
      tap(response => {
        if (response.id) {
          localStorage.setItem('userId', response.id); 
        }
      }),
      map(response => response.id || null),
      catchError(() => of(null))
    );
}

}
