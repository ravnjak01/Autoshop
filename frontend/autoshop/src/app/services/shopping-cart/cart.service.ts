import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private baseUrl = 'http://localhost:7000'; // prilagodi svoj backend URL

  constructor(private http: HttpClient) { }

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('my-auth-token');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'my-auth-token': token ? token : ''
    });
  }

  addToCart(productId: number, quantity: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/add-to-cart`, { productId, quantity }, { headers: this.getAuthHeaders() });
  }

  getMyCart(): Observable<any> {
    return this.http.get(`${this.baseUrl}/my-cart`, { headers: this.getAuthHeaders() });
  }

  removeFromCart(productId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/remove-from-cart/${productId}`, { headers: this.getAuthHeaders() });
  }
}
