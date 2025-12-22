import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { appRoutes as routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideHttpClient, withInterceptorsFromDi, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MyAuthInterceptor } from './core/services/auth/my-auth-interceptor.service';
import { MyErrorHandlingInterceptor } from './core/services/auth/my-error-handling-interceptor.service';
import { importProvidersFrom } from '@angular/core';
import { AppModule } from './app.module';
import { SharedModule } from './modules/shared/shared.module';
import { BlogModule } from './blog/blog.module';
import {DiscountModule} from './administration/discount-management/discount-managements.module';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
export const appConfig: ApplicationConfig = {

    providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideClientHydration(withEventReplay()),
    provideAnimations(),
    provideHttpClient(withInterceptorsFromDi()),

    // Interceptori
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MyAuthInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MyErrorHandlingInterceptor,
      multi: true
    },


    importProvidersFrom(
      AppModule,
      SharedModule,
      DiscountModule,
      BlogModule,
      MatSnackBarModule,
      BrowserAnimationsModule
    )
  ]



};
