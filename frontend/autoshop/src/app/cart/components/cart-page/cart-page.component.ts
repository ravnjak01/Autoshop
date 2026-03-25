import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { CartItemDTO } from '../../models/cart-item.dto';
import { Router, RouterModule } from '@angular/router';
import { MySnackbarHelperService } from '../../../modules/shared/snackbars/my-snackbar-helper.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
@Component({
  selector: 'app-cart',
  templateUrl: './cart-page.component.html',
  styleUrls: ['./cart-page.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    RouterModule
  ]
})
export class CartPageComponent implements OnInit {
  private destroyRef = inject(DestroyRef);

    isIncreaseDisabled(item:CartItemDTO):boolean {
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


  constructor(public cartService: CartService,private router:Router,private snackbar:MySnackbarHelperService) {}

  ngOnInit(): void {
    this.loadCart();
  }
loadCart(): void {
  this.cartService.getCart()
  .pipe(takeUntilDestroyed(this.destroyRef))
  .subscribe({
    next: (response) => {
      if (response && response.items) {
        this.cartItems = response.items;
      } else {
        this.cartItems = [];

        this.savedForLaterItems=response.savedItems || [];
      }

      this.isCartEmpty = this.cartItems.length === 0;
    },
    error: (err) => {
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
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe();
  }
  else
  {
    this.snackbar.showMessage('Not enough stock available', 'error');
  }


}

decreaseQuantity(item: CartItemDTO): void {
  if (item.quantity > 1) {
    const newQuantity = item.quantity - 1;

    this.cartService.updateQuantity(item.id, newQuantity)
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe({
      next: (updatedItem) => {
        item.quantity = updatedItem.quantity;
      }
    });
  } else {
    this.snackbar.showMessage('Quantity cannot be less than 1', 'error');
  }
}


 getTotal(): number {
   return this.cartItems.reduce((total, item) => {
     const price = item.priceAfterDiscount ?? item.price;
     return total + (price * item.quantity);
   }, 0);
  }

  goToCheckout(): void {
    this.router.navigate(['/checkout']);
  }

removeItem(item: CartItemDTO): void {
  const itemId = item.id;

  if (!itemId) {
    return;
  }

  this.cartService.removeFromCart(itemId)

  .pipe(takeUntilDestroyed(this.destroyRef))
  .subscribe({
    next: () => {
      this.cartService.loadCart();

      this.cartItems = this.cartItems.filter(i => i.id !== itemId);
    },
    error: (err) => {
      this.snackbar.showMessage('Failed to remove item from cart', 'error');
    }
  });
}



saveForLater(item: CartItemDTO): void {
  this.cartService.saveForLater(item.productId)
  .pipe(takeUntilDestroyed(this.destroyRef))
  .subscribe({
    next: () => {
      this.cartItems = this.cartItems.filter(i => i.productId !== item.productId);
      this.savedForLaterItems.push(item);
    },
  });
}

moveToCart(item: CartItemDTO): void {
  this.cartService.moveToCart(item.productId)
  .pipe(takeUntilDestroyed(this.destroyRef))
  . subscribe({
    next: () => {
      this.savedForLaterItems = this.savedForLaterItems.filter(i => i.productId !== item.productId);
      this.cartItems.push(item);
    },
  });
}

removeSavedItem(item: CartItemDTO): void {
  this.savedForLaterItems = this.savedForLaterItems.filter(i => i.productId !== item.productId);
}

}
