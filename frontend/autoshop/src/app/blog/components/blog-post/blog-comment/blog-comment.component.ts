

import {Component, Input, OnInit} from '@angular/core';
import {
  BlogCommentDto,
  BlogCommentGetByBlogIdService, BlogCommentsByBlogIdResponse
} from '../../../services/blog-endpoints/blog-comment-get-endpoint.service';
import {
  BlogCommentAddEndpointService,
  BlogCommentRequest
} from '../../../services/blog-endpoints/blog-comment-add-endpoint.service';
import {MyAuthService} from '../../../../core/services/auth/my-auth.service';

@Component({
  selector: 'app-blog-comments',
  templateUrl: './blog-comment.component.html',
  styleUrls: ['./blog-comment.component.css'],
  standalone: false,
})
export class BlogCommentsComponent implements OnInit {
  @Input() blogPostId!: number;
  comments: BlogCommentDto[] = [];
  totalCount: number = 0;
  newCommentContent: string = '';

  page: number = 1;
  pageSize: number = 5;

  isUserLoggedIn = false;

  constructor(
    private commentGetService: BlogCommentGetByBlogIdService,
    private commentAddService: BlogCommentAddEndpointService,
    public authService: MyAuthService
  ) {
    this.authService.isLoggedIn$.subscribe((loggedIn) => {
      this.isUserLoggedIn = loggedIn;
    });
  }

  ngOnInit() {
    this.loadComments();
  }

  loadComments(append: boolean = false) {
    this.commentGetService.handleAsync({
      blogId: this.blogPostId,
      page: this.page,
      pageSize: this.pageSize
    }).subscribe(
      (response: BlogCommentsByBlogIdResponse) => {

        if (append) {
          this.comments = [...this.comments, ...response.comments];
        } else {
          this.comments = response.comments;
        }

        this.totalCount = response.totalCount;
      },
      (error) => {}
    );
  }

  loadMore() {
    this.page++;
    this.loadComments(true);
  }

  addComment() {
    if (!this.newCommentContent.trim()) {
      return; // Exit the method if the comment is empty
    }

    const request: BlogCommentRequest = {
      blogPostId: this.blogPostId,
      content: this.newCommentContent
    };

    const formData = new FormData();
    formData.append('blogPostId', request.blogPostId.toString());
    formData.append('content', request.content);

    this.commentAddService.handleAsync(formData).subscribe(
      () => {
        this.page = 1;
        this.loadComments();
        this.newCommentContent = '';
      },
      (error) => {
      }
    );

  }
  clearComment(){
    this.newCommentContent = '';
  }
}
