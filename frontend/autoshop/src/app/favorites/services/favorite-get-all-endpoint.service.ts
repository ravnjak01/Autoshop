import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {tap} from 'rxjs/operators';
import {MyBaseEndpointAsync} from '../../core/helper/my-base-endpoint-async.interface';
import {MyConfig} from '../../my-config';
import {buildHttpParams} from '../../core/helper/http-params.helper';

export interface FavoriteGetAllRequest {
  sortBy?: string;
  pageNumber?: number;
pageSize?: number;

}
export interface Favorite {
  id: number;
  productId: number;
  name: string;
  description: string;
  price: number;
  imageUrl: string;
  brend: string;
  active: boolean;
}

export interface FavoriteGetAllResponse {
  favorites: Favorite[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class FavoritesGetAllService implements MyBaseEndpointAsync<FavoriteGetAllRequest, FavoriteGetAllResponse>{
  private apiUrl = `${MyConfig.api_address}/favorite/`;

  constructor(private httpClient: HttpClient) {
  }

  handleAsync(request: FavoriteGetAllRequest) {

      const cleanRequest: any = { ...request };

    let params = buildHttpParams(cleanRequest);

    return this.httpClient.get<FavoriteGetAllResponse>(`${this.apiUrl}`, {params}).pipe(
      tap(function () {}));
  }
}
