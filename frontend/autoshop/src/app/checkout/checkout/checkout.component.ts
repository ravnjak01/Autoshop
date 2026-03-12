import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MyAuthService } from '../../core/services/auth/my-auth.service';
import { ConfirmationModalComponent } from '../../confirmation-modal/confirmation-modal/confirmation-modal.component';
import { CartService } from '../../cart/services/cart.service';
import { MySnackbarHelperService } from '../../modules/shared/snackbars/my-snackbar-helper.service';
import { EMPTY, map, Observable, switchMap, take } from 'rxjs';
import { CartItemDTO } from '../../cart/models/cart-item.dto';


 /** 
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
      */

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ConfirmationModalComponent],
})
export class CheckoutComponent implements OnInit {
    checkoutForm: FormGroup;
  shippingFee = 10;

  cartItems$!: Observable<CartItemDTO[]>;
  subtotal$!: Observable<number>;
  vatAmount$!: Observable<number>;
  finalTotal$!: Observable<number>;

  showConfirmModal = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: MyAuthService,
    private cartService: CartService,
    private snackbar: MySnackbarHelperService
  ) {

     this.cartItems$ = this.cartService.cartItems$;
    this.subtotal$ = this.cartService.cartTotal$;
    this.vatAmount$ = this.subtotal$.pipe(map(subtotal => subtotal * 0.17));
    this.finalTotal$ = this.subtotal$.pipe(map(subtotal => subtotal + this.shippingFee));

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
    this.cartService.loadCart();
  }

  getItemTotal(item: CartItemDTO): number {
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

    this.finalTotal$.pipe(
      take(1),
      switchMap(finalTotal =>
        this.authService.getCurrentUserId().pipe(
          map(userId => ({ userId, finalTotal }))
        )
      ),
      switchMap(({ userId, finalTotal }) => {
        if (!userId) {
          this.router.navigate(['/login']);
          return EMPTY;
        }

        const checkoutData = {
          userId,
          adresa: {
            country: this.checkoutForm.value.country,
            city: this.checkoutForm.value.city,
            street: this.checkoutForm.value.street,
            postalCode: this.checkoutForm.value.postalCode,
            telephoneNumber: this.checkoutForm.value.phone || '',
            userId
          },
          paymentMethod: 'Cash',
          totalAmount: finalTotal
        };

        return this.cartService.checkout(checkoutData);
      })
    ).subscribe({
      next: () => {
        this.snackbar.showMessage('Your order has been successfully created!', 'success');
        this.checkoutForm.reset();
        this.cartService.clearCart().subscribe();
        setTimeout(() => this.router.navigate(['/home']), 2500);
      },
      error: () => this.snackbar.showMessage('Failed to place order', 'error')
    });
  }
}