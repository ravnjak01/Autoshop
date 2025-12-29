import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../../my-config';
import {buildHttpParams} from '../../../core/helper/http-params.helper';
import {MyBaseEndpointAsync} from '../../../core/helper/my-base-endpoint-async.interface';
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
  sortBy?: string;
  pageNumber?: number;
pageSize?: number;
stockQuantity?:boolean;

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
  imageUrl: string;
brend: string;
  stockQuantity:number;
  isFavorite: boolean;
}

export interface ProductGetAllResponse {
  products: Product[];
}

@Injectable({
  providedIn: 'root'
})
export class ProductsGetAllService implements MyBaseEndpointAsync<ProductGetAllRequest, ProductGetAllResponse>{
  private apiUrl = `${MyConfig.api_address}/product/filter/`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(request: ProductGetAllRequest) {

      const cleanRequest: any = { ...request };

      const categoryIds = cleanRequest.categoryIds;

      delete cleanRequest.categoryIds;



    let params = buildHttpParams(cleanRequest);

     if (categoryIds && Array.isArray(categoryIds) && categoryIds.length > 0) {
      categoryIds.forEach((id: number) => {
        if (id != null && id > 0) { // Provjeri da nije null/undefined
          params = params.append('categoryIds', id.toString());
        }
      });
    }

    return this.httpClient.get<ProductGetAllResponse>(`${this.apiUrl}`, {params}).pipe(
      tap(function () {}));
  }
}
