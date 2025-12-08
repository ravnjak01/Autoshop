import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../../my-config';
import {MyBaseEndpointAsync} from '../../../core/helper/my-base-endpoint-async.interface';

export interface DiscountPostUpdateOrInsertRequest {
  id?: number; // nullable => optional u TypeScriptu
  name: string;
  discountPercentage: number;
  startDate: string;
  endDate: string;
}


@Injectable({
  providedIn: 'root'
})
export class DiscountUpdateOrInsertEndpointService implements MyBaseEndpointAsync<FormData, void> {
  private apiUrl = `${MyConfig.api_address}/discount-post`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(request: FormData) {
    return this.httpClient.post<void>(`${this.apiUrl}`, request);
  }
}
