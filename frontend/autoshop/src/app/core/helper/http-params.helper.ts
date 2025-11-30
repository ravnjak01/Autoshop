import {HttpParams} from '@angular/common/http';

export function buildHttpParams<T extends Record<string, any>>(requestObject: T): HttpParams {
  let params = new HttpParams();

  Object.entries(requestObject).forEach(([key, value]) => {
    if (value !== undefined && value !== null) {
      if (Array.isArray(value)) {
        value.forEach((item) => {
          params = params.append(key, item.toString());
        });
      } else {
        params = params.set(key, value.toString());
      }
    }
  });

  return params;
}
