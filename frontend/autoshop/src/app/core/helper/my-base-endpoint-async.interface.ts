import {Observable} from 'rxjs';

export interface MyBaseEndpointAsync<TRequest = void, TResponse = void> {
  handleAsync(request: TRequest): Observable<TResponse>;
}

export interface MyBaseEndpointAsyncWithoutRequest<TResponse = void> {
  handleAsync(): Observable<TResponse>;
}
