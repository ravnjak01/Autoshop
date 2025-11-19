import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { MyAuthInterceptor } from './core/services/auth/my-auth-interceptor.service';
import { MyAuthService } from './core/services/auth/my-auth.service';
import { SharedModule } from './modules/shared/shared.module';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MyErrorHandlingInterceptor } from './core/services/auth/my-error-handling-interceptor.service';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { appRoutes as routes  } from './app.routes';


// Imports za Angular Material komponente i NgxSlider
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatCardModule } from '@angular/material/card';
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
import {MatButton} from '@angular/material/button';
import { RegisterComponent } from './register/register.component';
import {ProductListComponent} from './modules/products/products.component';
import { NgxSliderModule } from '@angular-slider/ngx-slider';
import { DiscountsComponent } from './modules/administration/discount/discount.component';
import { DiscountPostComponent } from './modules/administration/discount/discount-post/discount-post.component';
import {
    DiscountEditComponent
} from './modules/administration/discount/discount-post-editing/discount-posts-editing.component';
import {
    MatDatepicker,
    MatDatepickerInput,
    MatDatepickerModule,
    MatDatepickerToggle
} from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { DiscountCodesComponent } from './modules/administration/discount/discount-code/discount-codes.component';
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
    BrowserModule,
    CommonModule,
    BrowserAnimationsModule,
        FormsModule,
    ReactiveFormsModule,
    
    // Router sa rutama
    RouterModule.forRoot(routes),
    
    // Angular Material moduli
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatIconModule,
    MatPaginatorModule,
    MatSortModule,
    MatCardModule,
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
    MatCheckboxModule,
    MatDialogModule,
    MatToolbarModule,
    MatButtonModule,
    MatSelectModule,
    
    // Treći strani moduli
    InfiniteScrollModule,
    NgxSliderModule,  
  ],
  providers: [
   
    MyAuthService,

  ],

  

})
export class AppModule {
}
