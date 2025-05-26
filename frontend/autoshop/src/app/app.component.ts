
import {Component} from '@angular/core';
import {Router,NavigationEnd} from '@angular/router';

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

  constructor(private router: Router) {
    this.router.events.subscribe((event) => {
      if(event instanceof NavigationEnd) {
        const url=event.urlAfterRedirects;

        if(url.includes('/register') || url.includes('/forgot-password') ) {
          this.isLoginVisible = false;
        }
      this.isAdminPage = this.router.url.includes('/admin');
      }
    });
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
}
