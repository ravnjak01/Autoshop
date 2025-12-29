import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../../my-config';
import {MyBaseEndpointAsync} from '../../../core/helper/my-base-endpoint-async.interface';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FavoriteToggleEndpointService implements MyBaseEndpointAsync<number, boolean> {
  private apiUrl = `${MyConfig.api_address}/product/favorite/toggle`;

  constructor(private httpClient: HttpClient) {
  }
  handleAsync(id: number): Observable<boolean> {
    return this.httpClient.post<boolean>(`${this.apiUrl}/${id}`, id);
  }
}
