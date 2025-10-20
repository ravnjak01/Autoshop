import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private baseUrl = 'http://localhost:7000/api/products';

  constructor(private http: HttpClient) {}

  getAllProducts() {
    return this.http.get(`${this.baseUrl}`, { withCredentials: true });
  }

  getProductById(id: number) {
    return this.http.get(`${this.baseUrl}/${id}`, { withCredentials: true });
  }
}
