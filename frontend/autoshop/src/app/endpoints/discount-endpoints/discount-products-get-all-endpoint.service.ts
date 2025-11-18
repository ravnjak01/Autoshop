import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';
import {MyConfig} from '../../my-config';

export interface DiscountProducts {
  productId: number;
  productName: string;
  productCode: string;
  isSelected: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class DiscountProductGetAllService implements MyBaseEndpointAsync<number, DiscountProducts[]> {
  private apiUrl = `${MyConfig.api_address}/discounts`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(id: number) {
    return this.httpClient.get<DiscountProducts[]>(`${this.apiUrl}/${id}/products`);
  }
}

