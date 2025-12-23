import { Component, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, catchError, EMPTY, map, Observable, tap } from 'rxjs';
import {  CartItemDTO  } from '../models/cart-item.dto'
import { AddToCartDTO } from '../models/add-to-cart.dto';
import { UpdateCartItemDTO } from '../models/update-cart.dto';
import { CartResponseDTO } from '../models/cart-response.dto';
import { CurrencyPipe } from '@angular/common';
import { MySnackbarHelperService } from '../../modules/shared/snackbars/my-snackbar-helper.service';
@Injectable({
  providedIn: 'root',
  
})


export class CartService {
  private cartTotalSubject = new BehaviorSubject<number>(0);
  cartTotal$ = this.cartTotalSubject.asObservable();
  getItems(): any {
    throw new Error('Method not implemented.');
  }
  private baseUrl = 'http://localhost:7000/api/cart'; 

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
      console.error("Error during loading ", err);
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


  updateCartItem(itemId: number, quantity: number): Observable<any> {
    const updateData: UpdateCartItemDTO = { quantity };
    
    return this.http.put(
      `${this.baseUrl}/update/${itemId}`,
      updateData,
      { 
        withCredentials: true,
        headers: { 'Content-Type': 'application/json' }
      }
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
      console.log('Kompletna greška:', err);
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
          return { items: [], itemCount: 0, total: 0 };
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
      // Koristi se map da izvuče samo 'itemCount' iz objekta korpe
      map(cart => cart.itemCount)
    );
}

  checkout(): Observable<any> {
    return this.http.post(
      `http://localhost:7000/api/checkout`,
      {},
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