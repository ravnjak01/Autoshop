import {Category} from '../product-endpoints/product-get-all-endpoint.service';
import {Injectable} from '@angular/core';
import {MyBaseEndpointAsync, MyBaseEndpointAsyncWithoutRequest} from '../../helper/my-base-endpoint-async.interface';
import {MyConfig} from '../../my-config';
import {HttpClient} from '@angular/common/http';
import {MyCacheService} from '../../services/cache-service/my-cache.service';
import {of} from 'rxjs';
import {buildHttpParams} from '../../helper/http-params.helper';
import {tap} from 'rxjs/operators';

export interface CategoryGetAllResponse {
  categories: Category[];
}

@Injectable({
  providedIn: 'root'
})
export class CategoryGetAllService implements MyBaseEndpointAsyncWithoutRequest<CategoryGetAllResponse>{
  private apiUrl = `${MyConfig.api_address}/category`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync() {
    return this.httpClient.get<CategoryGetAllResponse>(`${this.apiUrl}`);
  }
}
