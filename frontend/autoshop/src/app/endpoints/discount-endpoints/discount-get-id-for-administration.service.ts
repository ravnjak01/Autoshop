import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../my-config';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';

export interface DiscountGetByIdForAdministrationResponse {
  id: number;
  name: string;
  discountPercentage: number;
  startDate: string; // ili Date, ako ćeš ga konvertovati u Date objekat u kodu
  endDate: string;   // isto kao gore
}

@Injectable({
  providedIn: 'root'
})
export class DiscountGetByIdForAdministrationService implements MyBaseEndpointAsync<number, DiscountGetByIdForAdministrationResponse> {
  private apiUrl = `${MyConfig.api_address}/administration/discount`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(id: number) {
    return this.httpClient.get<DiscountGetByIdForAdministrationResponse>(`${this.apiUrl}/${id}`);
  }
}
