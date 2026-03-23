import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import {DiscountsComponent} from './components/discount/discount.component';
import {DiscountPostComponent} from './components/discount-post/discount-post.component';
import {DiscountEditComponent} from './components/discount-post-editing/discount-posts-editing.component';
import {DiscountCodesComponent} from './components/discount-code/discount-codes.component';
import {DiscountCodeEditComponent} from './components/discount-code/discount-code-edit/discount-code-edit.component';
import {DiscountCategoryDialogComponent} from './components/discount-categories/discount-category.component';
import {DiscountProductDialogComponent} from './components/discount-products/discount-product.component';
import {SharedModule} from '../../modules/shared/shared.module';
import {MatTooltip} from '@angular/material/tooltip';
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
    SharedModule,
    MatTooltip
  ]
})
export class DiscountModule { }
