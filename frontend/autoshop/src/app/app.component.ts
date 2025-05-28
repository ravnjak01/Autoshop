
import {Component} from '@angular/core';
import {Router,NavigationEnd} from '@angular/router';
import { MyAuthService } from './services/auth-services/my-auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  standalone: false
})
export class AppComponent {
  isLoginVisible = false;
  title = 'Auto-shop ';
  currentRoute: string = '';
  isAdminPage = false;
  isUserLoggedIn = false;
  constructor(private router: Router, public authService: MyAuthService) {
    this.router.events.subscribe((event) => {
      if(event instanceof NavigationEnd) {
        const url=event.urlAfterRedirects;

        if(url.includes('/register') || url.includes('/forgot-password') ) {
          this.isLoginVisible = false;
        }
      this.isAdminPage = this.router.url.includes('/admin');
      }

   
    });
        this.authService.isLoggedIn$.subscribe((loggedIn) => {
      this.isUserLoggedIn = loggedIn;
    });
    
  }
  ngOnInit() {
  if (!this.authService.isLoggedIn()) {
    localStorage.removeItem('userName');
    localStorage.removeItem('userRole');
  }
}
    toggleLoginForm() {
      this.isLoginVisible = !this.isLoginVisible;
    }
     isLoginPage(): boolean {
      return this.currentRoute === '/login';
    }
    goToLogin() {
    this.router.navigate(['/login']);
  }
  onLogout() {
  this.authService.logout();
  }

}