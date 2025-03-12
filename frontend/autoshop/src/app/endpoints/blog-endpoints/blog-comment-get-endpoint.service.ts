import {Injectable} from '@angular/core';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';
import {MyConfig} from '../../my-config';
import {HttpClient} from '@angular/common/http';

export interface BlogCommentsByBlogIdResponse {
  comments: BlogCommentDto[];
}

export interface BlogCommentDto {
  id: number;
  blogPostId: number;
  userId?: number;
  anonymousName?: string;
  content: string;
  createdAtAgo: string;
}

@Injectable({
  providedIn: 'root'
})
export class BlogCommentGetByBlogIdService implements MyBaseEndpointAsync<number, BlogCommentsByBlogIdResponse>{
  private apiUrl = `${MyConfig.api_address}/blog-comment`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(id: number) {
  // Use the helper function here
    return this.httpClient.get<BlogCommentsByBlogIdResponse>(`${this.apiUrl}/${id}`);
  }
}
