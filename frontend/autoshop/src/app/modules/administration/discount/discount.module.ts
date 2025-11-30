import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';

import { DiscountPostComponent } from './discount-post/discount-post.component';
import { DiscountEditComponent } from './discount-post-editing/discount-posts-editing.component';
import { DiscountCodesComponent } from './discount-code/discount-codes.component';
import { DiscountCodeEditComponent } from './discount-code/discount-code-edit/discount-code-edit.component';
import { DiscountCategoryDialogComponent } from './discount-categories/discount-category.component';
import { DiscountProductDialogComponent } from './discount-products/discount-product.component';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { DiscountsComponent } from './discount.component';
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
     MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatTableModule,
    SharedModule
  ]
})
export class DiscountModule { }
