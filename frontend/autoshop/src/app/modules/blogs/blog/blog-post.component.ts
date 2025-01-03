
import {Component, Inject, OnInit} from '@angular/core';
import {
  BlogGetByIdForAdministrationService
} from '../../../endpoints/blog-endpoints/blog-get-id-for-administration.service';
import {ActivatedRoute, Router} from '@angular/router';

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

  constructor(
    private route: ActivatedRoute,
    public router: Router,
    private blogGetByIdService: BlogGetByIdForAdministrationService,
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
  }
}
