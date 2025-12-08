import { NgModule } from '@angular/core';


// Komponente
import { BlogDetailsComponent } from './components/blog-post/blog-post.component';
import { BlogCommentsComponent } from './components/blog-post/blog-comment/blog-comment.component';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { SharedModule } from '../modules/shared/shared.module';
import {BlogPostsComponent} from './components/blog-posts/blog-posts.component';
import {InfiniteScrollDirective} from 'ngx-infinite-scroll';



@NgModule({
  declarations: [
    BlogPostsComponent,
    BlogCommentsComponent,
  BlogDetailsComponent

  ],
  imports: [
    SharedModule,
    MatTableModule,
    MatButtonModule,
    InfiniteScrollDirective,
  ],
  exports: [
    BlogCommentsComponent,
    BlogDetailsComponent,
    BlogPostsComponent
  ]
})
export class BlogModule {}
