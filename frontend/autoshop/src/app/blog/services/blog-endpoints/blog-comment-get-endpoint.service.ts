import {Injectable} from '@angular/core';
import {MyBaseEndpointAsync} from '../../../core/helper/my-base-endpoint-async.interface';
import {MyConfig} from '../../../my-config';
import {HttpClient, HttpParams} from '@angular/common/http';

export interface BlogCommentsByBlogIdResponse {
  comments: BlogCommentDto[];
  totalCount: number;
}

export interface BlogCommentDto {
  id: number;
  blogPostId: number;
  username?: string;
  content: string;
  createdAtAgo: string;
}

export interface BlogCommentsRequest {
  blogId: number;
  page?: number;
  pageSize?: number;
}

@Injectable({
  providedIn: 'root'
})
export class BlogCommentGetByBlogIdService implements MyBaseEndpointAsync<BlogCommentsRequest, BlogCommentsByBlogIdResponse>{
  private apiUrl = `${MyConfig.api_address}/blog-comment`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(request: BlogCommentsRequest) {
    let params = new HttpParams()
      .set('blogId', request.blogId)
      .set('page', request.page ?? 1)
      .set('pageSize', request.pageSize ?? 10);

    return this.httpClient.get<BlogCommentsByBlogIdResponse>(this.apiUrl, { params });
  }
}
