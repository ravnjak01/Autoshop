import {Injectable} from "@angular/core";
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {MyAuthService} from "./my-auth.service";
import {MyPageProgressbarService} from '../../../modules/shared/progressbars/my-page-progressbar.service'
import {finalize, Observable} from 'rxjs';

@Injectable()
export class MyAuthInterceptor implements HttpInterceptor {

  constructor(private auth: MyAuthService,
              private progressBarService: MyPageProgressbarService
  ) {
  }

   intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
 
    const jwtToken = localStorage.getItem('jwtToken');


    if (jwtToken) {
      const cloned = req.clone({
        setHeaders: {
          Authorization: `Bearer ${jwtToken}`
        }
      });

      return next.handle(cloned);
    }
  
    return next.handle(req);
  }
}

