import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MyAuthService } from '../../core/services/auth/my-auth.service';
import { ConfirmationModalComponent } from '../../confirmation-modal/confirmation-modal/confirmation-modal.component';
import { CartService, CheckoutDTO } from '../../cart/services/cart.service';
import { MySnackbarHelperService } from '../../modules/shared/snackbars/my-snackbar-helper.service';
import {BehaviorSubject, combineLatest, EMPTY, map, Observable, switchMap, take} from 'rxjs';
import { CartItemDTO } from '../../cart/models/cart-item.dto';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import {PromoCodeValidateService} from '../services/promo-endpoint/promo-code-validation-endpoint.service';

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
  discountAmount$!: Observable<number>;
  finalTotal$!: Observable<number>;

  showConfirmModal = false;
  isSubmitting = false;
   private destroyRef = inject(DestroyRef);

  promoResponseMessage: string = '';
  isApplyingPromo: boolean = false;

  private discountPercentage$ = new BehaviorSubject<number>(0);

  discountPercentage: number = 0;

  appliedPromoCode: string | null = null;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: MyAuthService,
    private cartService: CartService,
    private snackbar: MySnackbarHelperService,
    private promoCodeService: PromoCodeValidateService
  ) {

     this.cartItems$ = this.cartService.cartItems$;
    this.subtotal$ = this.cartService.cartTotal$;
    this.vatAmount$ = this.subtotal$.pipe(map(subtotal => subtotal * 0.17));
    this.discountAmount$ = combineLatest([
      this.subtotal$,
      this.discountPercentage$
    ]).pipe(
      map(([subtotal, discount]) => subtotal * discount / 100)
    );

    this.finalTotal$ = combineLatest([
      this.subtotal$,
      this.discountAmount$
    ]).pipe(
      map(([subtotal, discountAmount]) => subtotal - discountAmount + this.shippingFee)
    );


    this.checkoutForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      street: ['', Validators.required],
      city: ['', Validators.required],
      postalCode: ['', Validators.required],
      country: ['', Validators.required],
      promoCode: ['']
    });
  }

  ngOnInit() {
    this.cartService.loadCart();
  }

  getItemTotal(item: CartItemDTO): number {
    const price = item.priceAfterDiscount ?? item.price;
    return price * item.quantity;
  }

  applyPromoCode() {

    const code = this.checkoutForm.get('promoCode')?.value;

    if (!code || code.trim() === '') {
      this.promoResponseMessage = 'Please enter promo code.';
      return;
    }

    this.isApplyingPromo = true;

    this.promoCodeService.handleAsync({ code })
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (res) => {
          this.isApplyingPromo = false;

          this.promoResponseMessage = res.message;

          if (res.isValid && res.discountPercentage) {
            this.discountPercentage = res.discountPercentage;
            this.discountPercentage$.next(res.discountPercentage);

            this.appliedPromoCode = code;
          } else {
            this.discountPercentage = 0;
            this.discountPercentage$.next(0);

            this.appliedPromoCode = null;
          }
        },
        error: () => {
          this.isApplyingPromo = false;
          this.promoResponseMessage = 'Error applying promo code.';
          this.appliedPromoCode = null;
        }
      });
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
    if (this.isSubmitting) return;
    this.showConfirmModal = false;
    this.isSubmitting = true;

    this.authService.getCurrentUserId().pipe(
    take(1),
    switchMap(userId => {
      if (!userId) {
        this.isSubmitting = false;
        this.router.navigate(['/login']);
        return EMPTY;
      }

        const checkoutData: CheckoutDTO = {
        userId,
        adresa: {
  country: this.checkoutForm.value.country,
  city: this.checkoutForm.value.city,
  street: this.checkoutForm.value.street,
  postalCode: this.checkoutForm.value.postalCode,
  telephoneNumber: this.checkoutForm.value.phone
},
        paymentMethod: 'Cash',
        promoCode: this.appliedPromoCode
      };

        return this.cartService.checkout(checkoutData);
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe({
      next: (response:any) => {
        this.snackbar.showMessage(`Order created! Total: ${response.total} USD`, 'success');
        this.checkoutForm.reset();
        this.cartService.loadCart();
        setTimeout(() => {
        this.isSubmitting = false;
        this.router.navigate(['/home']);
      }, 2500);
      },
      error: (err) => {
      this.isSubmitting = false;
      this.snackbar.showMessage('Failed to place order', 'error');
    }
    });
  }
}
