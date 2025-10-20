
/*
import {Component, Input, OnInit} from '@angular/core';
import {
  BlogCommentDto,
  BlogCommentGetByBlogIdService, BlogCommentsByBlogIdResponse
} from '../../../services/blog-endpoints/blog-comment-get-endpoint.service';
import {
  BlogCommentAddEndpointService,
  BlogCommentRequest
} from '../../../services/blog-endpoints/blog-comment-add-endpoint.service';

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
    if (!this.newCommentContent.trim()) {
      console.error('Comment cannot be empty');
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
        this.loadComments();
        this.newCommentContent = '';
      },
      (error) => {
        console.error('Error adding comment', error);
      }
    );

  }
  clearComment(){
    this.newCommentContent = '';
  }
}
*/