import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from "./app-routing.module";
import {AppComponent} from './app.component';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
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
import { MatTooltipModule } from '@angular/material/tooltip';
import {MatCheckbox} from '@angular/material/checkbox';
import {BlogPostComponent} from './modules/administration/blog-posts/blog-post/blog-post.component';
import {MatDialogActions, MatDialogContent} from '@angular/material/dialog';
import {MatToolbar} from '@angular/material/toolbar';
import {AdministrationComponent} from './modules/administration/administration.component';
import {HomePageComponent} from './modules/administration/home-page/home-page.component';
import {BlogListComponent} from './modules/blogs/blog-posts.component';
import {InfiniteScrollDirective} from 'ngx-infinite-scroll';
import {BlogDetailsComponent} from './modules/blogs/blog/blog-post.component';
import { BlogCommentsComponent } from './modules/blogs/blog-comment/blog-comment.component';
import {MatButton, MatIconButton} from '@angular/material/button';
import { RegisterComponent } from './register/register.component';
import {ProductListComponent} from './modules/products/products.component';
import {NgxSliderModule} from '@angular-slider/ngx-slider';
import {DiscountsComponent} from './modules/administration/discount/discount.component';
import {DiscountPostComponent} from './modules/administration/discount/discount-post/discount-post.component';
import {
  DiscountEditComponent
} from './modules/administration/discount/discount-post-editing/discount-posts-editing.component';
import {
  MatDatepicker,
  MatDatepickerInput,
  MatDatepickerModule,
  MatDatepickerToggle
} from '@angular/material/datepicker';
import {MatNativeDateModule} from '@angular/material/core';
import {DiscountCodesComponent} from './modules/administration/discount/discount-code/discount-codes.component';
import {
  DiscountCodeEditComponent
} from './modules/administration/discount/discount-code/discount-code-edit/discount-code-edit.component';
import {
  DiscountCategoryDialogComponent
} from './modules/administration/discount/discount-categories/discount-category.component';
import {
  DiscountProductDialogComponent
} from './modules/administration/discount/discount-products/discount-product.component';
@NgModule({
  declarations: [
    AppComponent,
    AdministrationComponent,
    HomePageComponent,
    BlogPostsComponent,
    BlogEditComponent,
    BlogPostComponent,
    BlogListComponent,
    BlogDetailsComponent,
    BlogCommentsComponent,
    RegisterComponent,
    ProductListComponent,
    DiscountsComponent,
    DiscountPostComponent,
    DiscountEditComponent,
    DiscountCodesComponent,
    DiscountCodeEditComponent,
    DiscountCategoryDialogComponent,
    DiscountProductDialogComponent
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
    InfiniteScrollDirective,
    MatButton,
    MatTooltipModule,
    NgxSliderModule,
    MatDatepickerToggle,
    MatDatepicker,
    MatDatepickerInput,
    MatDatepickerModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconButton,
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
