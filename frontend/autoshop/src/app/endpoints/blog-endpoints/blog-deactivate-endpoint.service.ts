import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../my-config';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BlogDeactivateEndpointService implements MyBaseEndpointAsync<number, void> {
  private apiUrl = `${MyConfig.api_address}/deactivate-blog`;

  constructor(private httpClient: HttpClient) {
  }
  handleAsync(id: number) {
    return this.httpClient.post<void>(`${this.apiUrl}/${id}`, id);
  }
}
