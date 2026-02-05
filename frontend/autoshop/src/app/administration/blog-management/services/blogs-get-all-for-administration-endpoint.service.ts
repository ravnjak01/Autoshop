import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyPagedRequest} from '../../../core/helper/my-paged-request';
import {MyConfig} from '../../../my-config';
import {buildHttpParams} from '../../../core/helper/http-params.helper';
import {MyBaseEndpointAsync} from '../../../core/helper/my-base-endpoint-async.interface';
import {MyPagedList} from '../../../core/helper/my-paged-list';
import {MyCacheService} from '../../../core/services/cache/my-cache.service';
import {of} from 'rxjs';
import {tap} from 'rxjs/operators';

export interface BlogsGetAllForAdministrationRequest extends MyPagedRequest {
  q?: string;
}

export interface BlogsGetAllForAdministrationResponse {
  id: number;
  title: string;
  authorName: string;
  publishedTime: Date | null;
  isPublished: boolean;
  active:boolean;
}

@Injectable({
  providedIn: 'root'
})
export class BlogsGetAllForAdministrationService implements MyBaseEndpointAsync<BlogsGetAllForAdministrationRequest, MyPagedList<BlogsGetAllForAdministrationResponse>> {
  private apiUrl = `${MyConfig.api_address}/administration/blogposts/filter`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(request: BlogsGetAllForAdministrationRequest) {


    const params = buildHttpParams(request);  // Use the helper function here
    return this.httpClient.get<MyPagedList<BlogsGetAllForAdministrationResponse>>(`${this.apiUrl}`, {params});
  }
}
