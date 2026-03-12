import { Component, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, catchError, EMPTY, map, Observable, tap } from 'rxjs';
import {  CartItemDTO  } from '../models/cart-item.dto'
import { CartResponseDTO } from '../models/cart-response.dto';
import { CurrencyPipe } from '@angular/common';
import { MySnackbarHelperService } from '../../modules/shared/snackbars/my-snackbar-helper.service';
import { MyConfig } from '../../my-config';
@Injectable({
  providedIn: 'root',
  
})


export class CartService {
  private cartTotalSubject = new BehaviorSubject<number>(0);
  cartTotal$ = this.cartTotalSubject.asObservable();

  private baseUrl = `${MyConfig.api_address}/api/cart`;

    private cartItemsSubject = new BehaviorSubject<CartItemDTO[]>([]);
  cartItems$ = this.cartItemsSubject.asObservable();
  constructor(private http: HttpClient,private snackbar:MySnackbarHelperService) { }

loadCart(): void {
  this.getCart().subscribe({
    next: (cartResponse) => {
      
      const items=cartResponse?.items || [];
      this.cartItemsSubject.next(items);
      this.cartTotalSubject.next(cartResponse?.total || 0);
    },
    error: (err) => {
      this.cartItemsSubject.next([]);
    }
  });
}

addToCart(productId: number, quantity: number = 1): Observable<CartItemDTO> {
    return this.http.post<CartItemDTO>(
      `${this.baseUrl}/add-to-cart`,
      { productId, quantity },
      { withCredentials: true }
    ).pipe(
     tap((response) => {
      
      const name=response.productName;
  
  this.snackbar.showMessage(`Product added to the cart`,'success');

      this.loadCart();
     })
     
    );
}

  
  removeFromCart(itemId: number): Observable<any> {
    return this.http.delete(
      `${this.baseUrl}/remove/${itemId}`,
      { withCredentials: true }
    ).pipe(
     tap(() => this.loadCart())

    );
  }

updateQuantity(itemId: number, quantity: number): Observable<CartItemDTO> {
  return this.http.put<CartItemDTO>(
    `${this.baseUrl}/update/${itemId}`,
    { quantity },
    { withCredentials: true }
  ).pipe(
    tap(() => this.loadCart()),
    catchError((err)=>{
      const message=err.error?.message || err.error ||'Not enough on the stock';
      this.snackbar.showMessage(message,'error');

      throw err;
    })
  );
}
  
getCart(): Observable<CartResponseDTO> {
  return this.http.get<CartResponseDTO>(`${this.baseUrl}/my-cart`, {withCredentials: true})
    .pipe(
      map((response) => {
       
        if (!response) {
          return { items: [], itemCount: 0, total: 0 ,savedItems : []};
        }
        return response;
      })
    );
}

getCartItems(): Observable<CartItemDTO[]> {
  return this.getCart().pipe(
    map(cart => cart.items)
  );
}
 
  clearCart(): Observable<any> {
    return this.http.delete(
      `${this.baseUrl}/clear`,
      { withCredentials: true }
    ).pipe(
     tap(() => this.loadCart())

    );
  }


 getCartItemCount(): Observable<number> {
    return this.getCart().pipe(
      map(cart => cart.itemCount)
    );
}

  checkout(data:any): Observable<any> {
    return this.http.post(
       `${MyConfig.api_address}/api/Checkout/checkout`,
      data,
      { 
        withCredentials: true,
        headers: { 'Content-Type': 'application/json' }
      }
    );
  }
  
  
  

  saveForLater(productId: number): Observable<any> {
  return this.http.post(`${this.baseUrl}/save-for-later/${productId}`, {}, { withCredentials: true })
    .pipe(tap(() => this.loadCart()));
}

moveToCart(productId: number): Observable<any> {
  return this.http.post(`${this.baseUrl}/move-to-cart/${productId}`, {}, { withCredentials: true })
    .pipe(tap(() => this.loadCart()));
}

}