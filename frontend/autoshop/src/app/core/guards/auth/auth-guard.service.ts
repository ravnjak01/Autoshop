import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router} from '@angular/router';
import {MyAuthService} from '../../services/auth/my-auth.service';

export class AuthGuardData {
  isAdmin?: boolean;
  isManager?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: MyAuthService, private router: Router) {
  }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const guardData = route.data as AuthGuardData;  


 
    
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/auth/login']);
      return false;
    }

   
    if (guardData.isAdmin && !this.authService.isAdmin()) {
      this.router.navigate(['/unauthorized']);
      return false;
    }

   
    if (guardData.isManager && !this.authService.isManager()) {
      this.router.navigate(['/unauthorized']);
      return false;
    }

    return true;
  }

}
