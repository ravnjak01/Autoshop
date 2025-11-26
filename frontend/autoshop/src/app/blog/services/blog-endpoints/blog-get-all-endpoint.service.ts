import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyConfig} from '../../../my-config';
import {buildHttpParams} from '../../../core/helper/http-params.helper';
import {MyBaseEndpointAsync} from '../../../core/helper/my-base-endpoint-async.interface';
import {MyCacheService} from '../../../core/services/cache/my-cache.service';
import {of} from 'rxjs';
import {tap} from 'rxjs/operators';

export interface BlogGetAllRequest {
  pageNumber: number;
  pageSize: number;
  searchQuery: string | null;
}
export interface BlogPost{
  id: number;
  title: string;
  author: string;
  publishedDate: Date;
  image: string | null;
}

export interface BlogGetAllResponse {
  totalCount: number;
  blogs: BlogPost[];
}

@Injectable({
  providedIn: 'root'
})
export class BlogsGetAllService implements MyBaseEndpointAsync<BlogGetAllRequest, BlogGetAllResponse>{
  private apiUrl = `${MyConfig.api_address}/blogposts/filter`;

  constructor(private httpClient: HttpClient, private cacheService: MyCacheService) {
  }

  handleAsync(request: BlogGetAllRequest, useCache: boolean = false, cacheTTL: number = 300000) {

    const cacheKey = `${request.pageNumber || 1}-${request.pageSize || 10}`;
    // Provjeri da li postoji keširana verzija
    if (useCache && this.cacheService.has(cacheKey)) {
      let data = this.cacheService.get<BlogGetAllResponse>(cacheKey)!;
      return of(data);
    }

    const params = buildHttpParams(request);  // Use the helper function here
    return this.httpClient.get<BlogGetAllResponse>(`${this.apiUrl}`, {params}).pipe(
      tap((data) => {
        if (useCache) {
          this.cacheService.set(cacheKey, data, cacheTTL);
        }
      }));
  }
}
