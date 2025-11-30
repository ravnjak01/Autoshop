import { Component, OnInit } from '@angular/core';
import {
  BlogGetAllRequest,
  BlogPost,
  BlogsGetAllService
} from '../../services/blog-endpoints/blog-get-all-endpoint.service';
import {Router} from '@angular/router';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
@Component({
  selector: 'app-blog-list',
  templateUrl: './blog-posts.component.html',
  standalone: true,
  styleUrls: ['./blog-posts.component.css'],
  
})
export class BlogPostsComponent implements OnInit {
  blogs: BlogPost[]= [];
  currentPage = 1;
  pageSize = 10;
  totalCount = 0;

  scrollDistance = 2; // Trigger the event a little later
  scrollUpDistance = 3; // Same for upward scrolling
  searchQuery = '';

  constructor(
    private blogService: BlogsGetAllService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadBlogs();
  }

  loadBlogs(): void {
    var request: BlogGetAllRequest = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize,
      searchQuery: this.searchQuery
    }
    this.blogService.handleAsync(request).subscribe(response => {
      this.totalCount = response.totalCount;
      this.blogs = [
        ...this.blogs,
        ...response.blogs.map(blog => ({
          ...blog,
          image: blog.image ? `data:image/jpeg;base64,${blog.image}` : null
        }))
      ];
    });
  }

  onScroll(): void {
    if (this.blogs.length < this.totalCount) {
      this.currentPage++;
      this.loadBlogs();
    }
  }

  openBlog(id: number) {
    this.router.navigate(['/blog', id]);
  }

  applyFilter($event: KeyboardEvent) {
    this.searchQuery = ($event.target as HTMLInputElement).value; // Capture the search input
    this.currentPage = 1; // Reset the page number for new search
    this.blogs = []; // Clear the previous blogs
    this.loadBlogs();
  }
}
