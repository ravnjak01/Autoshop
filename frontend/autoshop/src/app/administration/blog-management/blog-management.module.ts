import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';  // ← dodaj RouterModule
import { SharedModule } from '../../modules/shared/shared.module';

import { BlogEditComponent } from './components/blog-posts-editing/blog-posts-editing.component';
import { BlogPostComponent } from './components/blog/blog-post.component';
import { BlogPostsComponent } from './components/blogs/blog-posts.component';
import {
  MatCell, MatCellDef, MatColumnDef,
  MatHeaderCell, MatHeaderCellDef, MatHeaderRow,
  MatHeaderRowDef, MatRow, MatRowDef, MatTable
} from '@angular/material/table';
import { MatTooltip } from '@angular/material/tooltip';

const routes: Routes = [
  { path: '', component: BlogPostsComponent },
  { path: 'edit/:id', component: BlogEditComponent }
];

@NgModule({
  declarations: [
    BlogEditComponent,
    BlogPostComponent,
    BlogPostsComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),  // ← ovo je ključno, ne forRoot!
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
  ],
  exports: [
    BlogEditComponent,
    BlogPostComponent,
    BlogPostsComponent,
    RouterModule  // ← izvozi ako komponente koriste routerLink
  ]
})
export class BlogManagementModule { }