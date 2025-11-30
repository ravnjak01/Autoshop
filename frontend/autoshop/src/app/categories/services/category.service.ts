import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProductDTO } from '../../cart/models/product.dto';

export interface CategoryDTO {
id: number;

 
  name: string;
  code: string;


  description?: string; 

  products: ProductDTO[];
}

@Injectable({ providedIn: 'root' })
export class CategoryService {
  private apiUrl = 'http://localhost:7000/api/category';

  constructor(private http: HttpClient) {}

  getAllCategories(): Observable<CategoryDTO[]> {
    return this.http.get<CategoryDTO[]>(this.apiUrl);
  }
}
