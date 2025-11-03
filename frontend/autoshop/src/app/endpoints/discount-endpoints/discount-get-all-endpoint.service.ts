import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {MyPagedRequest} from '../../helper/my-paged-request';
import {MyConfig} from '../../my-config';
import {buildHttpParams} from '../../helper/http-params.helper';
import {MyBaseEndpointAsync} from '../../helper/my-base-endpoint-async.interface';
import {MyPagedList} from '../../helper/my-paged-list';
import {MyCacheService} from '../../services/cache-service/my-cache.service';
import {of} from 'rxjs';
import {tap} from 'rxjs/operators';

export interface DiscountGetAllRequest extends MyPagedRequest {
  q?: string;
}

export interface DiscountGetAllResponse {
  id: number;
  name: string;
  discountPercentage: number;
  startDate: Date;
  endDate: Date;
}

@Injectable({
  providedIn: 'root'
})
export class DiscountGetAllService implements MyBaseEndpointAsync<DiscountGetAllRequest, MyPagedList<DiscountGetAllResponse>> {
  private apiUrl = `${MyConfig.api_address}/discounts/filter`;

  constructor(private httpClient: HttpClient, private cacheService: MyCacheService) {
  }

  handleAsync(request: DiscountGetAllRequest, useCache: boolean = false, cacheTTL: number = 300000) {

    const cacheKey = `${request.q || ''}-${request.pageNumber || 1}-${request.pageSize || 10}`;
    // Provjeri da li postoji keširana verzija
    if (useCache && this.cacheService.has(cacheKey)) {
      let data = this.cacheService.get<MyPagedList<DiscountGetAllResponse>>(cacheKey)!;
      return of(data);
    }

    const params = buildHttpParams(request);  // Use the helper function here
    return this.httpClient.get<MyPagedList<DiscountGetAllResponse>>(`${this.apiUrl}`, {params}).pipe(
      tap((data) => {
        if (useCache) {
          this.cacheService.set(cacheKey, data, cacheTTL);
        }
      }));
  }
}
