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
//import { BlogPostsComponent } from './components/blogs/blog-posts.component';
//import { BlogEditComponent } from './components/blog-posts-editing/blog-posts-editing.component';
//import { BlogPostComponent } from './components/blog/blog-post.component';

//import { BlogDetailsComponent } from '../../blog/components/blog-post/blog-post.component';
import { MyDialogConfirmComponent } from '../../modules/shared/dialogs/my-dialog-confirm/my-dialog-confirm.component';
import { MyDialogSimpleComponent } from '../../modules/shared/dialogs/my-dialog-simple/my-dialog-simple.component';
import { MyInputTextComponent } from '../../modules/shared/my-reactive-forms/my-input-text/my-input-text.component';


@NgModule({
  declarations: [
 
   // BlogPostsComponent,
    //BlogEditComponent,
    //BlogPostComponent,
    //BlogCommentsComponent,
    //BlogDetailsComponent,

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
   // BlogPostsComponent,
    //BlogEditComponent,
    //BlogPostComponent,
  //  BlogDetailsComponent
  ]
})
export class BlogModule { }