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
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatCardModule } from '@angular/material/card';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogModule } from '@angular/material/dialog';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { NgxSliderModule } from '@angular-slider/ngx-slider';

import { BlogModule } from './blog/blog.module';
import { DiscountModule } from './modules/administration/discount/discount.module';
import { MatSelectModule } from '@angular/material/select';
import { BlogPostsComponent } from './blog/components/blog-posts/blog-posts.component';
import { DiscountsComponent } from './modules/administration/discount/discount.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { BlogManagementModule } from './administration/blog-management/blog-management.module';

@NgModule({

  declarations: [
  

  ],
  
  imports: [
       DiscountModule,
     SharedModule,
    BrowserModule,
    CommonModule,
    BrowserAnimationsModule,
        FormsModule,
    ReactiveFormsModule,
    
    BlogModule,
    BlogManagementModule,
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
    MatCheckboxModule,
    MatDialogModule,
    MatToolbarModule,
    MatButtonModule,
    MatSelectModule,
        MatDatepickerModule,
    MatNativeDateModule,
    
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
