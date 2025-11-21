import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// Importirajte Angular Material module
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';

// Komponente koje pripadaju ovom modulu
import { AdministrationComponent } from '../components/administration.component';



@NgModule({
  declarations: [
 
 

  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
       AdministrationComponent,
    // Importirajte sve potrebne Angular Material module
    MatTableModule,
    MatPaginatorModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatTooltipModule,
    MatIconModule,
    MatCardModule,
    MatCheckboxModule
  ],
  exports: [
    AdministrationComponent,

  ]
})
export class BlogModule { }