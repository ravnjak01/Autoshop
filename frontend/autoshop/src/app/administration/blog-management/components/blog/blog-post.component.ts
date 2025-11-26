
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import {BlogGetByIdForAdministrationService} from '../../../../blog/services/blog-endpoints/blog-get-id-for-administration.service';
import {Component, Inject, OnInit} from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-blog-post',
  templateUrl: './blog-post.component.html',
  styleUrls: ['./blog-post.component.css'],
  standalone: false,
  
})
export class BlogPostComponent implements OnInit {
  blog: any = {}; // Object to store blog data
  imageUrl: string | null = null; // URL for the image

  constructor(
    private dialogRef: MatDialogRef<BlogPostComponent>,
    private blogGetByIdService: BlogGetByIdForAdministrationService,
    @Inject(MAT_DIALOG_DATA) public data: { blogId: number }
  ) {}

  ngOnInit(): void {
    this.loadBlogData();
  }

  loadBlogData(): void {
    this.blogGetByIdService.handleAsync(this.data.blogId).subscribe({
      next: (blog) => {
        this.blog = blog; // Populate the blog object with the response
        this.imageUrl = blog.image
          ? `data:image/jpeg;base64,${blog.image}`
          : null; // Set the image URL if available
      },
      error: (error) => console.error('Error loading blog data', error),
    });
  }

  close(): void {
    this.dialogRef.close(); // Close the dialog
  }
}
