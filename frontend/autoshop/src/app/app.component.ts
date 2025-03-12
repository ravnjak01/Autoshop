import {Component} from '@angular/core';
import {Router} from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  standalone: false
})
export class AppComponent {
  title = 'Auto-shop ';

  isAdminPage = false;

  constructor(private router: Router) {
    this.router.events.subscribe(() => {
      this.isAdminPage = this.router.url.includes('/admin');
    });
  }
}
