import { Component, OnInit } from '@angular/core';
import { CartService } from '../../services/shopping-cart/cart.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
  standalone:false
})
export class CartComponent implements OnInit {
  cartItems: any[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(private cartService: CartService) {}

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    this.isLoading = true;
    this.cartService.getMyCart().subscribe({
      next: (data) => {
        this.cartItems = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Greška pri dohvaćanju korpe:', err);
        this.errorMessage = 'Neuspješno učitavanje korpe.';
        this.isLoading = false;
      }
    });
  }

  removeItem(productId: number): void {
    this.cartService.removeFromCart(productId).subscribe({
      next: () => {
        this.cartItems = this.cartItems.filter(item => item.productId !== productId);
      },
      error: (err) => {
        console.error('Greška pri uklanjanju artikla:', err);
      }
    });
  }
}
