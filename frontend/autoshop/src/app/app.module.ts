import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
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
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogModule } from '@angular/material/dialog';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
//import {BlogListComponent} from './administration/blog-management/components/';
import {InfiniteScrollDirective, InfiniteScrollModule} from 'ngx-infinite-scroll';
import {BlogDetailsComponent} from './blog/components/blog-post/blog-post.component';
import { BlogCommentsComponent } from './blog/components/blog-post/blog-comment/blog-comment.component';
import { NgxSliderModule } from '@angular-slider/ngx-slider';
import { MatSelectModule } from '@angular/material/select';
import { BlogEditComponent } from './administration/blog-management/components/blog-posts-editing/blog-posts-editing.component';
import { BlogPostsComponent } from './blog/components/blog-posts/blog-posts.component';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { BlogPostComponent } from './administration/blog-management/components/blog/blog-post.component';
import { DiscountModule } from './modules/administration/discount/discount.module';
@NgModule({

  declarations: [
  
    BlogPostsComponent,
    BlogEditComponent,
    BlogPostComponent,
    BlogDetailsComponent,
    BlogCommentsComponent,

  ],
  
  imports: [
       DiscountModule,
     SharedModule,
    BrowserModule,
    CommonModule,
    BrowserAnimationsModule,
        FormsModule,
    ReactiveFormsModule,
    
    // Router sa rutama
    RouterModule.forRoot(routes),
    
    // Angular Material moduli

    NgxSliderModule,
  MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatIconModule,
    MatPaginatorModule,
    MatSortModule,
    MatCardModule,
    MatTooltipModule,
    MatCheckboxModule, // ISPRAVNO
    MatDialogModule,   // ISPRAVNO
    MatToolbarModule,  // ISPRAVNO
    MatButtonModule,   // ISPRAVNO
    MatSelectModule,
        MatDatepickerModule,
    MatNativeDateModule,
    
    // Treći strani moduli
    InfiniteScrollModule,
   
  ],
  providers: [
   
    MyAuthService,

  ],



})
export class AppModule {
}
