import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { appRoutes as routes } from './app/app.routes';
import { HTTP_INTERCEPTORS, HttpClientModule, provideHttpClient, withInterceptors } from '@angular/common/http';
import { MyAuthInterceptor } from './app/core/services/auth/my-auth-interceptor.service';
import { MyAuthService } from './app/core/services/auth/my-auth.service';
import { appConfig } from './app/app.config';



bootstrapApplication(AppComponent,appConfig).catch(err => console.error(err));