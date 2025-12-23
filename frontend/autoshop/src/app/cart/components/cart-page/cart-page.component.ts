import { Component, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { CartItemDTO } from '../../models/cart-item.dto'; 
import { Router } from '@angular/router';
import { MySnackbarHelperService } from '../../../modules/shared/snackbars/my-snackbar-helper.service';

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
isIncreaseDisabled(item:any):boolean {
if(!item)
{
  return true;
}
if(!item.product)
{
  return true;
}

const quantity=Number(item.quantity ?? 0);
const stock=Number(item.product.stockQuantity ?? 0);

return quantity>=stock;
}
   cartItems: CartItemDTO[] = [];
  isCartEmpty: boolean = false;
savedForLaterItems: CartItemDTO[] = [];
goToCart() {
throw new Error('Method not implemented.');
}
  constructor(public cartService: CartService,private router:Router,private snackbar:MySnackbarHelperService) {}
 
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
  const newQuantity = item.quantity + 1;
  const currentQty=Number(item.quantity);
  const stock=Number(item.product.stockQuantity);

  if(newQuantity<stock){
    this.cartService.updateQuantity(item.id,newQuantity)
    .subscribe();
  }
  else
  {
    this.snackbar.showMessage('Dostigli ste limit zaliha', 'error');
  }

 
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


 getTotal(): number {
    return this.cartItems.reduce((sum, item) => sum + (item.price * item.quantity), 0);
  }

  goToCheckout(): void {
    this.router.navigate(['/checkout']);
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
