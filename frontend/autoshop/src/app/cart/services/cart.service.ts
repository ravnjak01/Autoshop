import { Component, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import {  CartItemDTO  } from '../models/cart-item.dto'
import { AddToCartDTO } from '../models/add-to-cart.dto';
import { UpdateCartItemDTO } from '../models/update-cart.dto';
import { CartResponseDTO } from '../models/cart-response.dto';
import { CurrencyPipe } from '@angular/common';
import { response } from 'express';

@Injectable({
  providedIn: 'root',
  
})


export class CartService {
  getItems(): any {
    throw new Error('Method not implemented.');
  }
  private baseUrl = 'http://localhost:7000/api/cart'; 

    private cartItemsSubject = new BehaviorSubject<CartItemDTO[]>([]);
  cartItems$ = this.cartItemsSubject.asObservable();
  constructor(private http: HttpClient) { }
loadCart(): void {
  this.getCart().subscribe({
    next: (cartResponse) => {
      // Ako backend vraća niz (lista od 1 elementa)
      const cart = Array.isArray(cartResponse) ? cartResponse[0] : cartResponse;
      const items = cart?.items || [];
      this.cartItemsSubject.next(items);
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
     tap(() => this.loadCart())
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
    );
  }

 
  
getCart(): Observable<CartResponseDTO> {
  return this.http.get<any>(`${this.baseUrl}/my-cart`, {withCredentials: true})
  .pipe(
    map((response) => {
     
      if (Array.isArray(response)) {
        if (response.length > 0) {
          return response[0];
        } else {
       
          return { items: [], itemCount: 0, total: 0 };
        }
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
    return new Observable(observer => {
      this.getCart().subscribe({
        next: (cart) => observer.next(cart.itemCount),
        error: (err) => observer.error(err)
      });
    });
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
  
  
  updateQuantity(productId: number, quantity: number): Observable<CartItemDTO> {
  return this.http.put<CartItemDTO>(
    `${this.baseUrl}/update-quantity/${productId}`,
    { quantity },
    { withCredentials: true }
  ).pipe(
    tap(() => this.loadCart())
  );
}

  
}