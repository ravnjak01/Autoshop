import {MyConfig} from '../../../my-config';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Injectable} from '@angular/core';

export interface PromoCodeValidateRequest {
  code: string;
}

export interface PromoCodeValidateResponse {
  isValid: boolean;
  discountPercentage?: number;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class PromoCodeValidateService {

  private apiUrl = `${MyConfig.api_address}/promo-validation`;

  constructor(private httpClient: HttpClient) {}

  handleAsync(request: PromoCodeValidateRequest): Observable<PromoCodeValidateResponse> {
    return this.httpClient.post<PromoCodeValidateResponse>(
      this.apiUrl,
      request
    );
  }
}
