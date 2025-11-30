import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../modules/shared/shared.module'; 

// Komponente koje pripadaju ovom modulu (moraju biti deklarisane)
import { BlogPostsComponent } from './components/blogs/blog-posts.component';
import { BlogEditComponent } from './components/blog-posts-editing/blog-posts-editing.component';
import { BlogPostComponent } from './components/blog/blog-post.component';


@NgModule({
  declarations: [
    BlogPostsComponent,
    BlogEditComponent,
    BlogPostComponent,
  ],
  imports: [
    CommonModule, 
    SharedModule, // SharedModule mora izvoziti MatDialogModule i CommonModule
  ],
  exports: [
    BlogPostsComponent,
    BlogEditComponent,
    BlogPostComponent,
  ]
})
export class BlogManagementModule { }