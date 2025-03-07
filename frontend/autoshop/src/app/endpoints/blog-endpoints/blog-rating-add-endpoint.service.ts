import {Injectable} from '@angular/core';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';
import {MyConfig} from '../../my-config';
import {HttpClient} from '@angular/common/http';

export interface BlogRatingRequest {
  blogPostId: number;
  rating: number;
}

@Injectable({
  providedIn: 'root'
})
export class BlogRatingAddEndpointService implements MyBaseEndpointAsync<FormData, void> {
  private apiUrl = `${MyConfig.api_address}/blog-rating`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(request: FormData) {
    return this.httpClient.post<void>(`${this.apiUrl}`, request);
  }
}

