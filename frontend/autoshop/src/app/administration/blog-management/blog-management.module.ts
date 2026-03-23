import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../modules/shared/shared.module';

// Komponente koje pripadaju ovom modulu (moraju biti deklarisane)
import { BlogEditComponent } from './components/blog-posts-editing/blog-posts-editing.component';
import {
  MatCell, MatCellDef,
  MatColumnDef,
  MatHeaderCell,
  MatHeaderCellDef,
  MatHeaderRow, MatHeaderRowDef,
  MatRow, MatRowDef,
  MatTable
} from '@angular/material/table';
import {MatTooltip} from '@angular/material/tooltip';
import {BlogPostComponent} from './components/blog/blog-post.component';
import {BlogPostsComponent} from './components/blogs/blog-posts.component';


@NgModule({
  declarations: [
    BlogEditComponent,
    BlogPostComponent,
    BlogPostsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    MatTable,
    MatHeaderCell,
    MatCell,
    MatTooltip,
    MatHeaderRow,
    MatRow,
    MatColumnDef,
    MatHeaderCellDef,
    MatCellDef,
    MatHeaderRowDef,
    MatRowDef,
    // SharedModule mora izvoziti MatDialogModule i CommonModule
  ],
  exports: [
    BlogEditComponent,
    BlogPostComponent,
    BlogPostsComponent,

  ]
})
export class BlogManagementModule { }
