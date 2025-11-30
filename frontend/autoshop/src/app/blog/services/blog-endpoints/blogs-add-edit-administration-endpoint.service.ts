import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../../my-config';
import {MyBaseEndpointAsync} from '../../../core/helper/my-base-endpoint-async.interface';

export interface BlogPostUpdateOrInsertRequest {
  id?: number;  // Nullable to allow null for insert operations
  title: string;
  content: string;
  image?: string;  // byte[] in C# can be represented as Uint8Array in TypeScript
  author: string;  // Changed from 'author' to 'authorId' as requested
  isPublished: boolean;
  active: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class BlogUpdateOrInsertEndpointService implements MyBaseEndpointAsync<FormData, void> {
  private apiUrl = `${MyConfig.api_address}/blog-post`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(request: FormData) {
    return this.httpClient.post<void>(`${this.apiUrl}`, request);
  }
}
