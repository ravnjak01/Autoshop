import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../../my-config';
import {MyBaseEndpointAsync} from '../../../core/helper/my-base-endpoint-async.interface';

@Injectable({
  providedIn: 'root'
})
export class BlogPublishEndpointService implements MyBaseEndpointAsync<number, void> {
  private apiUrl = `${MyConfig.api_address}/publish-blog`;

  constructor(private httpClient: HttpClient) {
  }
  handleAsync(id: number) {
    return this.httpClient.post<void>(`${this.apiUrl}/${id}`, id);
  }
}
