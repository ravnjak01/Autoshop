
import {Component, OnInit} from '@angular/core';
import {
  BlogGetByIdForAdministrationService
} from '../../services/blog-endpoints/blog-get-id-for-administration.service';
import {ActivatedRoute, Router} from '@angular/router';
import {BlogRatingGetByBlogIdService} from '../../services/blog-endpoints/blog-rating-get-endpoint.service';
import {
  BlogRatingAddEndpointService,
  BlogRatingRequest
} from '../../services/blog-endpoints/blog-rating-add-endpoint.service';
import { BlogCommentsComponent } from './blog-comment/blog-comment.component';

@Component({
  selector: 'app-blog-post-details',
  templateUrl: './blog-post.component.html',
  styleUrls: ['./blog-post.component.css'],
  standalone: false,
  
})
export class BlogDetailsComponent implements OnInit {
  blog: any = {};
  blogId: number ;
  imageUrl: string | null = null; // URL for the image
  averageRating: number | null = null;
  stars: boolean[] = [false, false, false, false, false];
  currentRating: number = 0;

  constructor(
    private route: ActivatedRoute,
    public router: Router,
    private blogGetByIdService: BlogGetByIdForAdministrationService,
    private blogRatingService: BlogRatingGetByBlogIdService,
    private ratingAddService: BlogRatingAddEndpointService
  ) {
    this.blogId = 0;
  }

  ngOnInit(): void {
    this.blogId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.blogId) {
      this.loadBlogData();
    }
  }

  loadBlogData(): void {
    this.blogGetByIdService.handleAsync(this.blogId).subscribe({
      next: (blog) => {
        this.blog = blog; // Populate the blog object with the response
        this.imageUrl = blog.image
          ? `data:image/jpeg;base64,${blog.image}`
          : null; // Set the image URL if available
      },
      error: (error) => console.error('Error loading blog data', error),
    });

    this.loadRating() ;
  }

  loadRating(){
    this.blogRatingService.handleAsync(this.blogId).subscribe(response => {
      this.averageRating = response.averageRating;
    });
  }
  addRating() {

    if (this.currentRating < 1 || this.currentRating > 5) {
      console.error('Rating must be between 1 and 5');
      return;
    }

    const request: BlogRatingRequest = {
      blogPostId: this.blogId,
      rating: this.currentRating
    };

    const formData = new FormData();
    formData.append('blogPostId', request.blogPostId.toString());
    formData.append('rating', request.rating.toString());

    this.ratingAddService.handleAsync(formData).subscribe(
      () => {
        this.loadRating()
      },
      (error) => {
        console.error('Error adding comment', error);
      }
    );
  }
  ratePost(rating: number): void {
    this.currentRating = rating;
    this.addRating();
  }

  
}

