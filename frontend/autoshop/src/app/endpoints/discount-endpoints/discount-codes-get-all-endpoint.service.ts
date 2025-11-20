import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';
import { MyBaseEndpointAsync } from '../../core/helper/my-base-endpoint-async.interface';

export interface DiscountCode {
  id: number;
  code: string;
  validFrom: string;
  validTo: string;
}

@Injectable({
  providedIn: 'root'
})
export class DiscountGetCodesService implements MyBaseEndpointAsync<number, DiscountCode[]> {
  private apiUrl = `${MyConfig.api_address}/discount-codes`;

  constructor(private httpClient: HttpClient) {}

  handleAsync(discountId: number) {
    return this.httpClient.get<DiscountCode[]>(`${this.apiUrl}/${discountId}`);
  }
}
