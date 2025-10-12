
import {Component} from '@angular/core';
import { Router, NavigationEnd, RouterOutlet, RouterLink } from '@angular/router';
import { MyAuthService } from './core/services/auth/my-auth.service';
import { CommonModule } from '@angular/common';
import { CartService } from './cart/services/cart.service';
import { CartItemDTO } from './cart/models/cart-item.dto';
import { CartSidebarComponent } from './cart/components/cart-sidebar/cart-sidebar.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone:true,
  imports: [CommonModule,RouterOutlet,RouterLink,CartSidebarComponent],

})
export class AppComponent {
clearCart() {
throw new Error('Method not implemented.');
}
  isLoginVisible = false;
  title = 'Auto-shop ';
  currentRoute: string = '';
  isAdminPage = false;
  isUserLoggedIn = false;
   isCartOpen: boolean = false;
cartItems: CartItemDTO[] = [];
cartItemCount: number = 0;
  constructor(private router: Router, public authService: MyAuthService,public cartService:CartService) {
    this.router.events.subscribe((event) => {
      if(event instanceof NavigationEnd) {
        const url=event.urlAfterRedirects;

        if(url.includes('/register') || url.includes('/forgot-password') ) {
          this.isLoginVisible = false;
        }
      this.isAdminPage = this.router.url.includes('/administration');
      }

   
    });
        this.authService.isLoggedIn$.subscribe((loggedIn) => {
      this.isUserLoggedIn = loggedIn;
    });
    
  }
  ngOnInit() {
  this.authService.checkAuth().subscribe({
    next: (isAuth) => {
      if (!isAuth) {
        this.authService.logout(); 
      }
    },
    error: () => {
      this.authService.logout();
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
  onLogout() {
  this.authService.logout();
  }
  

 

 
    addProductToCart(productId: number): void {
    this.cartService.addToCart(productId, 1).subscribe({
      next: (cartItem: CartItemDTO) => {
        console.log("Added to the cart:", cartItem);
        this.cartItems.push(cartItem);
      },
      error: (err:any) => console.error("Error during adding to the cart", err)
    });
  }

 

 

  
 loadCart(): void {
    this.cartService.getCart().subscribe({
      next: (data) => {
        this.cartItems = data.items; 
      },
      error: (err) => {
        console.error('Error during loading the cart', err);
      }
    });
  }
removeItem(productId: number): void {
    this.cartService.removeFromCart(productId).subscribe({
      next: () => this.loadCart(),
      error: (err) => console.error('Error while deleting', err)
    });
  }

   changeQuantity(item: CartItemDTO, increment: boolean): void {
      const newQuantity = increment ? item.quantity + 1 : item.quantity - 1;
      if (newQuantity < 1) return;
      this.cartService.updateCartItem(item.id, newQuantity).subscribe({
        next: () => this.loadCart(),
        error: (err) => console.error('Error while updating quantity', err)
      });
    }
  toggleCart(): void {
    this.isCartOpen = !this.isCartOpen;
    // Dodaj/ukloni klasu za body da sprečiš skrolovanje kada je cart otvoren
    if (this.isCartOpen) {
      document.body.classList.add('cart-open');
    } else {
      document.body.classList.remove('cart-open');
    }
  }

}
   
   
