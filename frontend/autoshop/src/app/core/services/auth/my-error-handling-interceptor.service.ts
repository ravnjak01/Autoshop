import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { MySnackbarHelperService } from '../../../modules/shared/snackbars/my-snackbar-helper.service';
import { MyAuthService } from '../../services/auth/my-auth.service'; // Provjeri putanju
import { Router } from '@angular/router';

@Injectable()
export class MyErrorHandlingInterceptor implements HttpInterceptor {
  
  constructor(
    private snackBar: MySnackbarHelperService,
    private authService: MyAuthService, 
    private router: Router               
  ) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        this.handleError(error);
        return throwError(() => error);
      })
    );
  }

  private handleError(error: HttpErrorResponse): void {
    let message = '';

    console.error('Error Intercepted:', error);

    if (error.error instanceof ErrorEvent) {
      message = 'A network or browser error occurred.';
    } else {
      switch (error.status) {
        case 400:
          message = 'Bad request (400). Please check your data.';
          break;
        case 401:
          // --- AUTOMATSKI LOGOUT LOGIKA ---
          message = 'Session expired. Please log in again.';
          this.authService.logout(); // Briše localStorage i resetuje stanje
          this.router.navigate(['/login']);
          break;
        case 403:
          message = 'Forbidden (403). Access denied.';
          break;
        case 404:
          message = 'Resource not found (404).';
          break;
        case 500:
          message = 'Internal server error (500).';
          break;
        case 0:
          message = 'Server is unreachable.';
          break;
        default:
          message = `Error: ${error.status}`;
      }
    }

    this.snackBar.showMessage(message, 'error', 5000);
  }
}