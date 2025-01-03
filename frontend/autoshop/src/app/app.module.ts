import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from "./app-routing.module";
import {AppComponent} from './app.component';
import {HTTP_INTERCEPTORS, HttpClient, HttpClientModule} from '@angular/common/http';
import {MyAuthInterceptor} from './services/auth-services/my-auth-interceptor.service';
import {MyAuthService} from './services/auth-services/my-auth.service';
import {SharedModule} from './modules/shared/shared.module';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MyErrorHandlingInterceptor} from './services/auth-services/my-error-handling-interceptor.service';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatOption, MatSelect} from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import {BlogPostsComponent} from './modules/administration/blog-posts/blog-posts.component';
import {BlogEditComponent} from './modules/administration/blog-posts/blog-posts-editing/blog-posts-editing.component';
import {MatCard} from '@angular/material/card';
import {MatCheckbox} from '@angular/material/checkbox';
import {BlogPostComponent} from './modules/administration/blog-posts/blog-post/blog-post.component';
import {MatDialogActions, MatDialogContent} from '@angular/material/dialog';
import {MatToolbar} from '@angular/material/toolbar';
import {AdministrationComponent} from './modules/administration/administration.component';
import {HomePageComponent} from './modules/administration/home-page/home-page.component';

@NgModule({
  declarations: [
    AppComponent,
    AdministrationComponent,
    HomePageComponent,
    BlogPostsComponent,
    BlogEditComponent,
    BlogPostComponent
  ],
  imports: [
    BrowserAnimationsModule, // Potrebno za animacije
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    SharedModule,
    MatFormField,
    MatSelect,
    MatOption,
    MatLabel,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatIconModule,
    MatPaginatorModule,
    MatSortModule,
    MatCard,
    MatCheckbox,
    MatDialogContent,
    MatDialogActions,
    MatToolbar,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MyAuthInterceptor,
      multi: true // Ensures multiple interceptors can be used if needed
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MyErrorHandlingInterceptor,
      multi: true // Dodaje ErrorHandlingInterceptor u lanac
    },
    MyAuthService,
    provideAnimationsAsync() // Ensure MyAuthService is available for the interceptor
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
