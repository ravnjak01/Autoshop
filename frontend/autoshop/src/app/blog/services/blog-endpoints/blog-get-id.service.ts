import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../../my-config';
import {MyBaseEndpointAsync} from '../../../core/helper/my-base-endpoint-async.interface';

export interface BlogGetByIdResponse {
  id: number;
  title: string;
  content: string;
  authorName: string;
  publishedTime: Date | null;
  isPublished: boolean;
  active:boolean;
  image: string;
}

@Injectable({
  providedIn: 'root'
})
export class BlogGetByIdService implements MyBaseEndpointAsync<number, BlogGetByIdResponse> {
  private apiUrl = `${MyConfig.api_address}/blogpost`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(id: number) {
    return this.httpClient.get<BlogGetByIdResponse>(`${this.apiUrl}/${id}`);
  }
}
