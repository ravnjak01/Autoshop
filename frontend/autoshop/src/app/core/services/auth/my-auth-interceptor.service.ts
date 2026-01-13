import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { MyPageProgressbarService } from "../../../modules/shared/progressbars/my-page-progressbar.service";
import { MyAuthService } from "./my-auth.service";

@Injectable()
export class MyAuthInterceptor implements HttpInterceptor {

  constructor(
    private auth: MyAuthService,
    private progressBarService: MyPageProgressbarService
  ) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    
    // Lista ruta koje NE trebaju token
    const skipAuth = [
      '/api/auth/login',
      '/api/auth/register',
      '/api/auth/forgot-password',
      '/api/auth/reset-password'
    ];

    // Provjeri da li je trenutna ruta u listi za preskakanje
    const shouldSkipAuth = skipAuth.some(url => req.url.includes(url));

    if (shouldSkipAuth) {
      // Za auth rute, samo šalji sa credentials
      return next.handle(req.clone({ withCredentials: true }));
    }

    // Za sve ostale rute, dodaj token
    const jwtToken = localStorage.getItem('jwtToken');
    
    const headers: any = {};
    if (jwtToken) {
      headers['Authorization'] = `Bearer ${jwtToken}`;
    }

    const cloned = req.clone({
      withCredentials: true,
      setHeaders: headers
    });

    return next.handle(cloned);
  }
}