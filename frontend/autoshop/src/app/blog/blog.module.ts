import { NgModule } from '@angular/core';

import { BlogRoutingModule } from './blog-routing.module';

// Komponente
import { BlogDetailsComponent } from './components/blog-post/blog-post.component';
import { BlogCommentsComponent } from './components/blog-post/blog-comment/blog-comment.component';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { BlogPostComponent } from '../administration/blog-management/components/blog/blog-post.component';
import { SharedModule } from '../modules/shared/shared.module';



@NgModule({
  declarations: [
  
    BlogCommentsComponent,
  BlogDetailsComponent
 
  ],
  imports: [
    SharedModule,
    BlogRoutingModule,
    MatTableModule,   
    MatButtonModule,
  ],
  exports: [
    BlogCommentsComponent,
    BlogDetailsComponent
  ]
})
export class BlogModule {}
