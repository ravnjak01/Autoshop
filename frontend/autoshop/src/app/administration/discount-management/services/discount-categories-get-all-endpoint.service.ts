import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {MyBaseEndpointAsync} from '../../../core/helper/my-base-endpoint-async.interface';
import {MyConfig} from '../../../my-config';

export interface DiscountCategories {
  categoryId: number;
  categoryName: string;
  isSelected: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class DiscountCategoryGetAllService implements MyBaseEndpointAsync<number, DiscountCategories[]> {
  private apiUrl = `${MyConfig.api_address}/discounts`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(id: number) {
    return this.httpClient.get<DiscountCategories[]>(`${this.apiUrl}/${id}/categories`);
  }
}

