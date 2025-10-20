import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { BlogRoutingModule } from './blog-routing.module';

// Komponente
import { BlogDetailsComponent } from './components/blog-post/blog-post.component';
import { BlogCommentsComponent } from './components/blog-post/blog-comment/blog-comment.component';
import { BlogPostsComponent } from './components/blog-posts/blog-posts.component';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';



@NgModule({
  declarations: [
    BlogDetailsComponent,
    BlogCommentsComponent,
    BlogPostsComponent,
 
  ],
  imports: [
    CommonModule,
    FormsModule,
    BlogRoutingModule,
    MatTableModule,   
    MatButtonModule,
  ],
  exports: [
    BlogDetailsComponent,
    BlogCommentsComponent,
  BlogPostsComponent
  ]
})
export class BlogModule {}
