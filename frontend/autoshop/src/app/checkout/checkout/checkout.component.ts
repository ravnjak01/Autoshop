import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MyAuthService } from '../../core/services/auth/my-auth.service';
import { ConfirmationModalComponent } from '../../confirmation-modal/confirmation-modal/confirmation-modal.component';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ConfirmationModalComponent],
})
export class CheckoutComponent implements OnInit {
  checkoutForm: FormGroup;
  subtotalPrice = 0;
  shippingFee = 10;
  finalTotalPrice = 0;
  vatAmount = 0;
  cartItems: any[] = [];
  showConfirmModal = false;
  successMessage = '';
  errorMessage = '';

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private authService: MyAuthService) {
    this.checkoutForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      street: ['', Validators.required],
      city: ['', Validators.required],
      postalCode: ['', Validators.required],
      country: ['', Validators.required],
    });
  }

  ngOnInit() {
    this.http.get<any>('http://localhost:7000/api/cart/my-cart', { withCredentials: true })
      .subscribe({
        next: (cart) => {
          if (cart && cart.items) {
            this.cartItems = cart.items;
            this.subtotalPrice = this.cartItems.reduce((sum, item) => sum + item.total, 0);
            this.vatAmount = this.subtotalPrice * 0.17;
            this.finalTotalPrice = this.subtotalPrice + this.shippingFee;
          } else {
            this.cartItems = [];
          }
        },
        error: (err) => console.error('Error when reaching the cart', err)
      });
  }
  getItemTotal(item: any): number {
  return item.price * item.quantity;
}

  onSubmit() {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login'], { queryParams: { returnUrl: '/checkout' } });
      return;
    }
    if (this.checkoutForm.invalid) return;
    this.showConfirmModal = true;
  }

  placeOrder() {
    this.showConfirmModal = false;
    this.authService.getCurrentUserId().subscribe({
      next: (userId) => {
        if (!userId) {
          this.router.navigate(['/login']);
          return;
        }
        const checkoutData = {
          userId: userId,
          adresa: {
            country: this.checkoutForm.value.country,
            city: this.checkoutForm.value.city,
            street: this.checkoutForm.value.street,
            postalCode: this.checkoutForm.value.postalCode,
            telephoneNumber: this.checkoutForm.value.phone || '',
            userId: userId
          },
          paymentMethod: 'Cash',
          totalAmount: this.finalTotalPrice
        };

        this.http.post('http://localhost:7000/api/Checkout/checkout', checkoutData, { withCredentials: true })
          .subscribe({
            next: () => {
              this.successMessage = 'Your order has been successfully created! Thank you for your trust.';
              this.checkoutForm.reset();
              setTimeout(() => this.router.navigate(['/home']), 2500);
            },
            error: () => alert('Failed to place order. Please try again.')
          });
      },
      error: (err) => console.error('Error getting user ID:', err)
    });
  }
}