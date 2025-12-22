import {Injectable} from '@angular/core';
import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {catchError} from 'rxjs/operators';
import {Observable, throwError} from 'rxjs';
import {MySnackbarHelperService} from '../../../modules/shared/snackbars/my-snackbar-helper.service'

@Injectable()
export class MyErrorHandlingInterceptor implements HttpInterceptor {
  constructor(private snackBar: MySnackbarHelperService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    console.log('Interceptor je pokrenut!');
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
      
        this.handleError(error);

        
        return throwError(() => error);
      })
    );
  }

private handleError(error: HttpErrorResponse): void {
  let message = '';


    console.error('Error:',error)
  if (error.error instanceof ErrorEvent) {
    // Client-side greška
    message = 'A network or browser error occurred. Please try again.';
  } else {
    // Server-side greške
    switch (error.status) {
      case 400:
        message = 'Bad request (400). Please check your input data.';
        break;
      case 401:
        message = 'Unauthorized (401). Please log in again.';
        break;
      case 403:
        message = 'Forbidden (403). You do not have permission for this action.';
        break;
      case 404:
        message = 'Resource not found (404). The requested data does not exist.';
        break;
      case 500:
        message = 'Internal server error (500). We are working on a fix.';
        break;
      case 0:
        message = 'Server is unreachable. Please check your internet connection.';
        break;
      default:
        message = `An unexpected error occurred: ${error.status}`;
    }
  }

  this.snackBar.showMessage(message, 'error',5000);
}
}
