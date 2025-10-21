import { Component, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { CartItemDTO } from '../../models/cart-item.dto'; 
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart',
  templateUrl: './cart-page.component.html',
  styleUrls: ['./cart-page.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule
  ]
})
export class CartPageComponent implements OnInit {
   cartItems: CartItemDTO[] = [];
  isCartEmpty: boolean = false;
savedForLaterItems: CartItemDTO[] = [];
goToCart() {
throw new Error('Method not implemented.');
}
  constructor(public cartService: CartService,private router:Router) {}
 
  ngOnInit(): void {
    this.loadCart();
  }
loadCart(): void {
  this.cartService.getCart().subscribe({
    next: (response) => {
      if (response && response.items) {
        this.cartItems = response.items;
      } else {
        console.warn('No items in cart response');
        this.cartItems = [];
      }
      
      this.isCartEmpty = this.cartItems.length === 0;
    },
    error: (err) => {
      console.error('Error loading cart', err);
      this.cartItems = [];
      this.isCartEmpty = true;
    }
  });
}

  increaseQuantity(item: CartItemDTO): void {
    this.cartService.updateQuantity(item.id, item.quantity + 1).subscribe({
      next: (updatedItem) => item.quantity = updatedItem.quantity
    });
  }

 decreaseQuantity(item: CartItemDTO): void {
  if (item.quantity > 1) {
    const newQuantity = item.quantity - 1;

    this.cartService.updateQuantity(item.id, newQuantity).subscribe({
      next: (updatedItem) => item.quantity = updatedItem.quantity,
      error: (err) => console.error('Error decreasing quantity', err)
    });
  } else {
    console.warn('Količina ne može biti manja od 1');
  }
}

 getTotal(): number {
    return this.cartItems.reduce((sum, item) => sum + (item.price * item.quantity), 0);
  }

  goToCheckout(): void {
    this.router.navigate(['/checkout']);
  }
  
  removeItem(item: CartItemDTO): void {
    this.cartService.removeFromCart(item.productId).subscribe({
    next: () => {
      this.cartItems = this.cartItems.filter(i => i.productId !== item.productId);
      this.isCartEmpty = this.cartItems.length === 0;
    },
    error: (err) => console.error('Error removing item', err)
  });
  }


saveForLater(item: CartItemDTO): void {
  this.cartService.saveForLater(item.productId).subscribe({
    next: () => {
      this.cartItems = this.cartItems.filter(i => i.productId !== item.productId);
      this.savedForLaterItems.push(item);
    },
    error: (err) => console.error('Error saving item for later:', err)
  });
}

moveToCart(item: CartItemDTO): void {
  this.cartService.moveToCart(item.productId).subscribe({
    next: () => {
      this.savedForLaterItems = this.savedForLaterItems.filter(i => i.productId !== item.productId);
      this.cartItems.push(item);
    },
    error: (err) => console.error('Error moving item back to cart:', err)
  });
}

removeSavedItem(item: CartItemDTO): void {
  this.savedForLaterItems = this.savedForLaterItems.filter(i => i.productId !== item.productId);
}

}
