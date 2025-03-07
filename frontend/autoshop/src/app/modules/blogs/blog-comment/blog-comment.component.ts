import {Component, Input, OnInit} from '@angular/core';
import {
  BlogCommentDto,
  BlogCommentGetByBlogIdService, BlogCommentsByBlogIdResponse
} from '../../../endpoints/blog-endpoints/blog-comment-get-endpoint.service';
import {
  BlogCommentAddEndpointService,
  BlogCommentRequest
} from '../../../endpoints/blog-endpoints/blog-comment-add-endpoint.service';

@Component({
  selector: 'app-blog-comments',
  templateUrl: './blog-comment.component.html',
  styleUrls: ['./blog-comment.component.css'],
  standalone: false,
})
export class BlogCommentsComponent implements OnInit {
  @Input() blogPostId!: number;
  comments: BlogCommentDto[] = [];
  newCommentContent: string = '';
  isAddingComment: boolean = false;

  constructor(
    private commentGetService: BlogCommentGetByBlogIdService,
    private commentAddService: BlogCommentAddEndpointService,
  ) {}

  ngOnInit() {
    this.loadComments();
  }

  loadComments() {
    this.commentGetService.handleAsync(this.blogPostId).subscribe(
      (response: BlogCommentsByBlogIdResponse) => {
        this.comments = response.comments;
      },
      (error) => {
        console.error('Error loading comments', error);
      }
    );
  }

  addComment() {
    const request: BlogCommentRequest = {
      blogPostId: this.blogPostId,
      content: this.newCommentContent
    };

    const formData = new FormData();
    formData.append('blogPostId', request.blogPostId.toString());
    formData.append('content', request.content);

    this.commentAddService.handleAsync(formData).subscribe(
      () => {
        this.loadComments();
        this.newCommentContent = '';
        this.isAddingComment = false;
      },
      (error) => {
        console.error('Error adding comment', error);
      }
    );

  }
}
