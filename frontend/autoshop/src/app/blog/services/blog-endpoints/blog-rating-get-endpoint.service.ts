import {Injectable} from '@angular/core';
import {MyBaseEndpointAsync} from '../../../core/helper/my-base-endpoint-async.interface';
import {MyConfig} from '../../../my-config';
import {HttpClient} from '@angular/common/http';

export interface BlogRatingByBlogIdResponse  {
  averageRating: number;
}

@Injectable({
  providedIn: 'root'
})
export class BlogRatingGetByBlogIdService implements MyBaseEndpointAsync<number, BlogRatingByBlogIdResponse>{
  private apiUrl = `${MyConfig.api_address}/blog-rating`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(id: number) {
    // Use the helper function here
    return this.httpClient.get<BlogRatingByBlogIdResponse>(`${this.apiUrl}/${id}`);
  }
}
