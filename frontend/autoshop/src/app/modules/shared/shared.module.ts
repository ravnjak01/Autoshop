import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MyDialogSimpleComponent } from './dialogs/my-dialog-simple/my-dialog-simple.component';
import { MyDialogConfirmComponent } from './dialogs/my-dialog-confirm/my-dialog-confirm.component';
import { MyPageProgressbarComponent } from './progressbars/my-page-progressbar/my-page-progressbar.component';
import { MyInputTextComponent } from './my-reactive-forms/my-input-text/my-input-text.component';
import { MyDropdownComponent } from './my-reactive-forms/my-dropdown/my-dropdown.component';
import { MyInputNumberComponent } from './my-reactive-forms/my-input-number/my-input-number.component';

// Angular Material Moduli
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';

const MATERIAL_MODULES = [
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatProgressBarModule,
    MatToolbarModule,
    MatSortModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCardModule,
    MatCheckboxModule,
];

const COMPONENTS = [
    MyDialogSimpleComponent,
    MyDialogConfirmComponent,
    MyPageProgressbarComponent,
    MyInputTextComponent,
    MyDropdownComponent,
    MyInputNumberComponent,
];

@NgModule({
    declarations: [
        ...COMPONENTS
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        ...MATERIAL_MODULES
    ],
    exports: [
        CommonModule, // Ovo eksportuje DatePipe
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        ...COMPONENTS,
        ...MATERIAL_MODULES
    ]
})
export class SharedModule { }