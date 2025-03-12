import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../my-config';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';

export interface BlogGetByIdForAdministrationResponse {
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
export class BlogGetByIdForAdministrationService implements MyBaseEndpointAsync<number, BlogGetByIdForAdministrationResponse> {
  private apiUrl = `${MyConfig.api_address}/administration/blogpost`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(id: number) {
    return this.httpClient.get<BlogGetByIdForAdministrationResponse>(`${this.apiUrl}/${id}`);
  }
}
