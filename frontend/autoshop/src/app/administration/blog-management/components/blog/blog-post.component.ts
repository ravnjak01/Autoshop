
import { MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {BlogGetByIdForAdministrationService} from '../../services/blog-get-id-for-administration.service';
import {Component, Inject, OnInit} from '@angular/core';
@Component({
  selector: 'app-blog-post',
  templateUrl: './blog-post.component.html',
  styleUrls: ['./blog-post.component.css'],
  standalone: false

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
        this.blog = blog;
        this.imageUrl = blog.image ?? null
      },
    });
  }

  close(): void {
    this.dialogRef.close(); // Close the dialog
  }
}
