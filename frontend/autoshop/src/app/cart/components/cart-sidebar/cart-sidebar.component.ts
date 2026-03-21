import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import { CartService } from '../../services/cart.service';
import { CommonModule } from '@angular/common';
import { CartItemDTO } from '../../models/cart-item.dto';
import { Router } from '@angular/router';
import { MyDialogConfirmComponent } from '../../../modules/shared/dialogs/my-dialog-confirm/my-dialog-confirm.component';
import { MatDialog } from '@angular/material/dialog';

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


    constructor(public cartService: CartService, private router: Router,private dialog:MatDialog) {}


   ngOnInit() {

  
  this.cartService.cartItems$.subscribe(items => {

    this.cartItems = items;
  });
  
  this.cartService.loadCart();
}

 removeItem(item: CartItemDTO): void {
  const itemId = item.id;
  if (!itemId) return;

  this.cartService.removeFromCart(itemId).subscribe({
    next: () => {
    }
  });
}

 clearCart(): void {
  const dialogRef = this.dialog.open(MyDialogConfirmComponent, {
    data: {
      title: 'Clear Cart',
      message: 'Are you sure you want to clear the cart?',
      confirmButtonText: 'Clear'
    }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      this.cartService.clearCart().subscribe();
    }
  });
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


 

increaseQuantity(item: CartItemDTO): void {
  if(item.quantity>=item.stockQuantity)
  {
    return; 
  }
  const newQuantity = item.quantity + 1;

  this.cartService.updateQuantity(item.id!, newQuantity).subscribe({
    next: (updatedItem) => {
      item.quantity = updatedItem.quantity; 
    },
    error: (err) => {
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
    });
  } else {
    
    
  }
}



}

