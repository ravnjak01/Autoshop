import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {RouterLink} from '@angular/router';
import {MyDialogSimpleComponent} from './dialogs/my-dialog-simple/my-dialog-simple.component';
import {
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogModule,
  MatDialogTitle
} from '@angular/material/dialog';
import {MatButton, MatButtonModule, MatIconButton} from '@angular/material/button';
import {MyDialogConfirmComponent} from './dialogs/my-dialog-confirm/my-dialog-confirm.component';
import {MatIcon} from '@angular/material/icon';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import {MyPageProgressbarComponent} from './progressbars/my-page-progressbar/my-page-progressbar.component';
import {MatProgressBar} from '@angular/material/progress-bar';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatSortModule} from '@angular/material/sort';
import {MatPaginatorModule} from '@angular/material/paginator';
import {MyInputTextComponent} from './my-reactive-forms/my-input-text/my-input-text.component';
import {MatError, MatFormField, MatLabel} from "@angular/material/form-field";
import {MatInput} from '@angular/material/input';
import {MyDropdownComponent} from './my-reactive-forms/my-dropdown/my-dropdown.component';
import {MatOption, MatSelect} from '@angular/material/select';
import {MatSpinner} from '@angular/material/progress-spinner';
import {MyInputNumberComponent} from './my-reactive-forms/my-input-number/my-input-number.component';

@NgModule({
  declarations: [
    MyDialogSimpleComponent,
    MyDialogConfirmComponent,
    MyPageProgressbarComponent,
    MyInputTextComponent,
    MyDropdownComponent,
    MyInputNumberComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatProgressBarModule,
    MatToolbarModule,
    MatDialogModule,
    MatButtonModule,
    MatSortModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatProgressSpinnerModule
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MyPageProgressbarComponent,
    MyInputTextComponent,
    MyDropdownComponent,
    MyInputNumberComponent
  ]
})
export class SharedModule { }
