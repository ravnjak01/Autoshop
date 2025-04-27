import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../my-config';
import {buildHttpParams} from '../../helper/http-params.helper';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';
import {tap} from 'rxjs/operators';

export interface Category {
  id: number;
  name: string;
  code: string;
}

export interface ProductGetAllRequest {
  searchQuery?: string;
  categoryIds?: number[];
  minPrice?: number;
  maxPrice?: number;
  sortBy?: string; // "price" or "createdDate"
}
export interface Product {
  id: number;
  name: string;
  code: string;
  description: string;
  price: number;
  createdAt: string;
  categoryId?: number;
  category: Category;
}

export interface ProductGetAllResponse {
  products: Product[];
}

@Injectable({
  providedIn: 'root'
})
export class ProductsGetAllService implements MyBaseEndpointAsync<ProductGetAllRequest, ProductGetAllResponse>{
  private apiUrl = `${MyConfig.api_address}/product/filter`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(request: ProductGetAllRequest) {

    const params = buildHttpParams(request);  // Use the helper function here
    return this.httpClient.get<ProductGetAllResponse>(`${this.apiUrl}`, {params}).pipe(
      tap(function () {}));
  }
}
