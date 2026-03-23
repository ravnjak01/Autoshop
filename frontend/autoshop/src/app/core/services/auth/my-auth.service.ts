import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, Observable, of, switchMap, tap } from 'rxjs';
import { MyAuthInfo } from '../../../modules/shared/dtos/my-auth-info';
import { LoginTokenDto } from '../../../modules/shared/dtos/login-token-dto';
import {MySignalRService} from '../../../core/services/signalr/my-signal-r.service';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { MyConfig } from '../../../my-config';
import { jwtDecode } from 'jwt-decode';
export interface LoginResponse {
  token: string;
  expiration?: string;  
  username?: string;    
  roles?: UserRole[];
}


export interface UserInfoDto{
  id:string;
  username:string;
  email:string;
  roles:UserRole[];
}

export enum UserRole {
  Admin = 'Admin',
  Manager = 'Manager',
  Customer = 'Customer'
}

@Injectable({ providedIn: 'root' })
export class MyAuthService {
   private apiUrl = `${MyConfig.api_address}/api/auth`; 
    private userUrl = `${MyConfig.api_address}/api/user`;
    private isLoggedInSubject = new BehaviorSubject<boolean>(this.hasStoredLogin());
  isLoggedIn$ = this.isLoggedInSubject.asObservable();


  getMyAuthInfo(): MyAuthInfo | null {
  const rolesString = localStorage.getItem('userRoles');
  const username = localStorage.getItem('userName');
 if (!username || !rolesString) return null;

  return {
    username,
    roles: JSON.parse(rolesString) as UserRole[]
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
login(credentials: { username: string; password: string }): Observable<LoginResponse> {
  return this.httpClient
    .post<LoginResponse>(`${this.apiUrl}/login`, credentials)
    .pipe(
      tap((response: LoginResponse) => {
        if (response && response.token) {
          // 1. Spasi token
          localStorage.setItem('jwtToken', response.token);
          localStorage.setItem('userName', response.username || '');

          // 2. Dekodiraj token da izvučeš role (ako nisu već u response.roles)
          try {
            const decoded: any = jwtDecode(response.token);
            // .NET Identity obično stavlja role pod ovaj dugački URL ključ
            const decodedRoles = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
            
            // Provjeri jesu li role stigle u odgovoru ili ih uzimamo iz tokena
            let rolesToStore: string[] = [];
            
            if (response.roles) {
              rolesToStore = response.roles;
            } else if (decodedRoles) {
              // Ako je samo jedna rola, biće string, ako ih je više biće niz
              rolesToStore = Array.isArray(decodedRoles) ? decodedRoles : [decodedRoles];
            }

            // 3. Spasi uloge u localStorage
            localStorage.setItem('userRoles', JSON.stringify(rolesToStore));
          } catch (error) {
            console.error('Greška pri dekodiranju tokena:', error);
          }

          this.isLoggedInSubject.next(true); // Obavijesti ostatak aplikacije
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
      .get<{ username: string; roles: UserRole[] }>(`${this.userUrl}/me`, { headers })
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

    this.httpClient.post(`${this.apiUrl}/logout`, {}).subscribe({
      next: () => {
        localStorage.clear();
           this.isLoggedInSubject.next(false);
      this.router.navigate(['/login']);
      },
      error: () => {
        localStorage.clear();
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
  return this.httpClient.post(`${this.apiUrl}/reset-password`, data);
}

  register(data: { email: string, username:string,password: string }): Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/register`, data);
  }
  isLoggedIn(): boolean {
    return this.isLoggedInSubject.value;
  }

  isAdmin(): boolean {
  const rolesString = localStorage.getItem('userRoles');
  if (!rolesString) return false;

  const roles: UserRole[] = JSON.parse(rolesString);
  return roles.includes(UserRole.Admin);
}
isManager(): boolean {

  const rolesString = localStorage.getItem('userRoles');
  if (!rolesString) return false;

  try {
    const roles: UserRole[] = JSON.parse(rolesString);
    return roles.includes(UserRole.Manager);
}
catch(e){
  return false;
}
}
  getUserData(): Observable<UserInfoDto> {
    return this.httpClient.get<UserInfoDto>(`${this.apiUrl}/user`);
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
        .get<{ id: string; username: string; email: string; roles: UserRole[] }>(`${this.userUrl}/me`, { headers })
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
