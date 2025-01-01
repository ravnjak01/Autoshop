import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../my-config';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';

export interface BlogPostUpdateOrInsertRequest {
  id?: number;  // Nullable to allow null for insert operations
  title: string;
  content: string;
  image?: string;  // byte[] in C# can be represented as Uint8Array in TypeScript
  author: string;  // Changed from 'author' to 'authorId' as requested
  isPublished: boolean;
  active: boolean;
}

export interface BlogUpdateOrInsertResponse {
  id: number;  // Required integer for ID
  title: string;  // Required string for Title
  content: string;  // Required string for Content
  authorName: string;  // Required string for AuthorName
  publishedTime?: Date;  // Optional Date (nullable)
  isPublished: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class BlogUpdateOrInsertEndpointService implements MyBaseEndpointAsync<FormData, BlogUpdateOrInsertResponse> {
  private apiUrl = `${MyConfig.api_address}/blog-post`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(request: FormData) {
    return this.httpClient.post<BlogUpdateOrInsertResponse>(`${this.apiUrl}`, request);
  }
}
