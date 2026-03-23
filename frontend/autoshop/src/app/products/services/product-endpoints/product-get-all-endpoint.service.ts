import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../../my-config';
import {buildHttpParams} from '../../../core/helper/http-params.helper';
import {MyBaseEndpointAsync} from '../../../core/helper/my-base-endpoint-async.interface';
import {tap} from 'rxjs/operators';
import { ProductDTO } from '../../../cart/models/product.dto';

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
  sortBy?: string;
  pageNumber?: number;
  pageSize?: number;
}

export interface Product {
  id: number;
  name: string;
  price: number;
  priceAfterDiscount?: number; // nova cijena nakon globalnog popusta
  badgeDiscountPercentage?: number;
  categoryName: string;
  imageUrl: string;
  brend: string;
  isFavorite: boolean;
  stockQuantity:number;
}

export interface ProductGetAllResponse {
  products: ProductDTO[];
  promoCode?: string;
}

@Injectable({
  providedIn: 'root'
})
export class ProductsGetAllService implements MyBaseEndpointAsync<ProductGetAllRequest, ProductGetAllResponse>{
  private apiUrl = `${MyConfig.api_address}/products`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(request: ProductGetAllRequest) {

      const cleanRequest: ProductGetAllRequest = { ...request };

      const categoryIds = cleanRequest.categoryIds;

      delete cleanRequest.categoryIds;



    let params = buildHttpParams(cleanRequest);

     if (categoryIds && Array.isArray(categoryIds) && categoryIds.length > 0) {
      categoryIds.forEach((id: number) => {
        if (id != null && id > 0) {
          params = params.append('categoryIds', id.toString());
        }
      });
    }

    return this.httpClient.get<ProductGetAllResponse>(`${this.apiUrl}`, {params}).pipe(
      tap(function () {}));
  }
}
