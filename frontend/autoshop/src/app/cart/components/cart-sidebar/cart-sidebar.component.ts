import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import { CartService } from '../../services/cart.service';
import { CommonModule } from '@angular/common';
import { CartItemDTO } from '../../models/cart-item.dto';
import { Router } from '@angular/router';

@Component({
  selector: 'cart-sidebar',
  templateUrl: './cart-sidebar.component.html',
  styleUrls: ['./cart-sidebar.component.css'],
  standalone: true,
  imports:[CommonModule]
})

export class CartSidebarComponent  {
  isCartOpen = false;
totalPrice: string|number = 0;
cartItems: CartItemDTO[] = [];
 @Output() closeSidebar = new EventEmitter<void>();
goToCart() {
 this.closeSidebar.emit();  
  this.router.navigate(['/cart']);
}


    constructor(public cartService: CartService, private router: Router) {}


   ngOnInit() {

  
  this.cartService.cartItems$.subscribe(items => {

    this.cartItems = items;
  });
  
  this.cartService.loadCart();
}

 removeItem(item: CartItemDTO): void {
  const itemId = item.id;
  
  if (!itemId) {
    console.error('Error: item doesnt have id.');
    return;
  }

  this.cartService.removeFromCart(itemId).subscribe({
    next: () => {
      console.log('Product removed:', item.productName);
      this.cartService.loadCart();
      
      this.cartItems = this.cartItems.filter(i => i.id !== itemId);
    },
    error: (err) => {
      console.error('Error during removing product from the cart', err);
    }
  });
}

 clearCart(): void {
  if (confirm('Are you sure you want to clear the cart?')) {
    this.cartService.clearCart().subscribe({
      next: () => {
        console.log("Cart cleared");
        this.cartItems = [];
      },
      error: (err) => {
        console.error("Error during clearing the cart", err);
      }
    });
  }
}

  getTotalPrice(): number {
    if (!this.cartItems || this.cartItems.length === 0) {
    return 0;
  }
    return this.cartItems.reduce((total, item) => {
      return total + (item.price * item.quantity);
    }, 0);
  }

  getTotalItems(): number {
    return this.cartItems.reduce((total, item) => {
      return total + item.quantity;
    }, 0);
  }

   isCartEmpty(): boolean {
    return this.cartItems.length === 0;
  }


 
     checkout(): void {
  this.cartService.checkout().subscribe({
    next: (res) => {
      alert('Checkout successfull!'); 
      this.cartService.clearCart(); 
      this.isCartOpen = false;      
    },
    error: (err) => {
      console.error('Error during checkout', err);
      alert('Checkout wasnt successfull.');
    }
  });
}

increaseQuantity(item: CartItemDTO): void {
  const newQuantity = item.quantity + 1;

  this.cartService.updateQuantity(item.id!, newQuantity).subscribe({
    next: (updatedItem) => {
      item.quantity = updatedItem.quantity; 
    },
    error: (err) => {
      console.error('Error during increasing quantity', err);
    }
  });
}
decreaseQuantity(item: CartItemDTO): void {
  if (item.quantity > 1) {
    const newQuantity = item.quantity - 1;

    this.cartService.updateQuantity(item.id, newQuantity).subscribe({
      next: (updatedItem) => {
        item.quantity = updatedItem.quantity;
      },
      error: (err) => console.error('Error decreasing quantity', err)
    });
  } else {
    
    
  }
}



}

