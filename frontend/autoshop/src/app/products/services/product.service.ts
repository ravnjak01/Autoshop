import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProductCreateDTO, ProductUpdateDTO } from '../../cart/models/product.dto';

export interface ProductCreate {
  name: string;
  price: number;
  description?: string;
  categoryId?: number;
}

export interface ProductUpdate extends ProductCreate {
  id: number;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private baseUrl = 'http://localhost:7000/api/product'; 

  constructor(private http: HttpClient) {}

  getAllProducts(): Observable<any> {
    return this.http.get(`${this.baseUrl}`, { withCredentials: true });
  }

  getProductById(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/${id}`, { withCredentials: true });
  }


  addProduct(product: ProductCreateDTO): Observable<any> {
    return this.http.post(`${this.baseUrl}`, product, { withCredentials: true });
  }


  updateProduct(id: number, product: ProductUpdateDTO): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}`, product, { withCredentials: true });
  }


  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`, { withCredentials: true });
  }

  GetAllImages(): Observable<string[]> {
return this.http.get<string[]>(`${this.baseUrl}/images`, { withCredentials: true });

  }
}
