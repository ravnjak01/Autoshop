import {Injectable} from '@angular/core';
import {MyBaseEndpointAsyncWithoutRequest} from '../../../core/helper/my-base-endpoint-async.interface';
import {MyConfig} from '../../../my-config';
import {HttpClient} from '@angular/common/http';

export interface PromoCodeResponse {
  isValid: boolean;
  promoCode: string;
  discountPercentage?: number;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class PromoCodeService  implements MyBaseEndpointAsyncWithoutRequest<PromoCodeResponse>{
  private apiUrl = `${MyConfig.api_address}/promo-code`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync() {
    return this.httpClient.get<PromoCodeResponse>(`${this.apiUrl}`);
  }
}
