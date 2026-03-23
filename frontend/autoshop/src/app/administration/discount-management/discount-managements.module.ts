import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router'; // DODAJ OVO
import { ReactiveFormsModule } from '@angular/forms';

// Angular Material Importi
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatTooltip } from '@angular/material/tooltip';

// Komponente
import { DiscountsComponent } from './components/discount/discount.component';
import { DiscountPostComponent } from './components/discount-post/discount-post.component';
import { DiscountEditComponent } from './components/discount-post-editing/discount-posts-editing.component';
import { DiscountCodesComponent } from './components/discount-code/discount-codes.component';
import { DiscountCodeEditComponent } from './components/discount-code/discount-code-edit/discount-code-edit.component';
import { DiscountCategoryDialogComponent } from './components/discount-categories/discount-category.component';
import { DiscountProductDialogComponent } from './components/discount-products/discount-product.component';
import { SharedModule } from '../../modules/shared/shared.module';

// DEFINICIJA RUTA ZA OVAJ MODUL
const routes: Routes = [
  { 
    path: '', 
    component: DiscountsComponent 
  },
  { 
    path: 'codes', 
    component: DiscountCodesComponent 
  },
  { 
    path: 'edit/:id', 
    component: DiscountEditComponent 
  }
];

@NgModule({
  declarations: [
    DiscountsComponent,
    DiscountPostComponent,
    DiscountEditComponent,
    DiscountCodesComponent,
    DiscountCodeEditComponent,
    DiscountCategoryDialogComponent,
    DiscountProductDialogComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes), // OBAVEZNO DODAJ OVO ZA LAZY LOADING
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatTableModule,
    MatTooltip
  ]
})
export class DiscountModule { }