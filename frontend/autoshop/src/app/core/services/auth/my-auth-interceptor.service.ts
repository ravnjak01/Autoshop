import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { MyPageProgressbarService } from "../../../modules/shared/progressbars/my-page-progressbar.service";
import { MyAuthService } from "./my-auth.service";
import { MyConfig } from "../../../my-config";
@Injectable()
export class MyAuthInterceptor implements HttpInterceptor {

  constructor(
    private auth: MyAuthService,
    private progressBarService: MyPageProgressbarService
  ) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    
    const isApiUrl = req.url.startsWith(MyConfig.api_address);

    // Lista ruta koje NE trebaju token
    const skipAuth = [
      '/api/auth/login',
      '/api/auth/register',
      '/api/auth/forgot-password',
      '/api/auth/reset-password'
    ];

    const shouldSkipAuth = skipAuth.some(url => req.url.includes(url));

   if (!isApiUrl || shouldSkipAuth) {
      return next.handle(req.clone({ withCredentials: true }));
    }

    const jwtToken = localStorage.getItem('jwtToken');
    
    if (jwtToken) {
      const cloned = req.clone({
        withCredentials: true,
        setHeaders: {
          Authorization: `Bearer ${jwtToken}`
        }
      });
      return next.handle(cloned);
    }

    return next.handle(req.clone({ withCredentials: true }));
  }
}