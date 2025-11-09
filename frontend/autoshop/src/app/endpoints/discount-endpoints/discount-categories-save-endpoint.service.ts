import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';
import {MyConfig} from '../../my-config';

export interface DiscountCategoryRequest {
  discountId: number;
  categoryIds: number[];
}

@Injectable({
  providedIn: 'root'
})
export class DiscountCategorySaveService implements MyBaseEndpointAsync<DiscountCategoryRequest, void> {
  private apiUrl = `${MyConfig.api_address}`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(request: DiscountCategoryRequest) {
    return this.httpClient.post<void>(`${this.apiUrl}/${request.discountId}/categories`, request);
  }
}

