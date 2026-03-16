import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProductCreateDTO, ProductUpdateDTO } from '../../cart/models/product.dto';
import { MyConfig } from '../../my-config';




export interface ProductReadDTO {
  id: number;
  name: string;
  price: number;
  description?: string; 
  imageUrl?: string;
  sku?: string;
  brend?: string;
  code?: string;
  stockQuantity: number;
  active: boolean;

  categoryId: number;
  categoryName?: string;

  priceAfterGlobalDiscount?: number;
  badgeDiscountPercentage?: number;

  isFavorite: boolean;

  createdAt?: Date | string; 
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private baseUrl = `${MyConfig.api_address}/api/product`; 

  constructor(private http: HttpClient) {}

 

  getProductById(id: number): Observable<ProductReadDTO> {
    return this.http.get<ProductReadDTO>(`${this.baseUrl}/${id}`, { withCredentials: true });
  }


  addProduct(product: ProductCreateDTO): Observable<ProductReadDTO> {
    return this.http.post<ProductReadDTO>(`${this.baseUrl}`, product, { withCredentials: true });
  }


  updateProduct(id: number, product: ProductUpdateDTO): Observable<ProductReadDTO> {
    return this.http.put<ProductReadDTO>(`${this.baseUrl}/${id}`, product, { withCredentials: true });
  }


  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`, { withCredentials: true });
  }

  GetAllImages(): Observable<string[]> {
return this.http.get<string[]>(`${this.baseUrl}/images`, { withCredentials: true });

  }
}
