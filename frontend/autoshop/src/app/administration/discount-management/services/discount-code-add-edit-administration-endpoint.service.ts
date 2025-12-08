import {Injectable} from '@angular/core';
import {MyBaseEndpointAsync} from '../../../core/helper/my-base-endpoint-async.interface';
import {MyConfig} from '../../../my-config';
import {HttpClient} from '@angular/common/http';

export interface DiscountCodePostRequest {
  id?: number;
  code: string;
  discountId: number;
  validFrom: string;
  validTo: string;
}

@Injectable({ providedIn: 'root' })
export class DiscountCodeUpdateService implements MyBaseEndpointAsync<FormData, void> {
  private apiUrl = `${MyConfig.api_address}/discount-code-post`;

  constructor(private httpClient: HttpClient) {}

  handleAsync(request: FormData) {
    return this.httpClient.post<void>(this.apiUrl, request);
  }
}
