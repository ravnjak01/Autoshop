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
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogModule } from '@angular/material/dialog';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { NgxSliderModule } from '@angular-slider/ngx-slider';
import { HttpClient } from '@microsoft/signalr';
import { CheckoutComponent } from './checkout/checkout/checkout.component';
import { ConfirmationModalComponent } from './confirmation-modal/confirmation-modal/confirmation-modal.component';
import { ProductManagementComponent } from './administration/products/product-management/product-management.component';
import { CategoriesComponent } from './categories/components/categories.component';


@NgModule({

  declarations: [

  
    
  

      
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
