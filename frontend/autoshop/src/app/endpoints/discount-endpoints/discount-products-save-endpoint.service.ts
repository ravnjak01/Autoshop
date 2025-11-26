import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {MyBaseEndpointAsync} from '../../core/helper/my-base-endpoint-async.interface';
import {MyConfig} from '../../my-config';

export interface DiscountProductsRequest {
  discountId: number;
  productIds: number[];
}

@Injectable({
  providedIn: 'root'
})
export class DiscountProductSaveService implements MyBaseEndpointAsync<DiscountProductsRequest, void> {
  private apiUrl = `${MyConfig.api_address}/discounts`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(request: DiscountProductsRequest) {
    return this.httpClient.post<void>(`${this.apiUrl}/save-products`, request);
  }
}

