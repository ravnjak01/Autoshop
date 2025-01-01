import {Component, Inject, OnInit} from '@angular/core';
import {
  BlogUpdateOrInsertEndpointService,
  BlogPostUpdateOrInsertRequest
} from '../../../../endpoints/blog-endpoints/blogs-add-edit-administration-endpoint.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  BlogGetByIdForAdministrationService
} from '../../../../endpoints/blog-endpoints/blog-get-id-for-administration.service';

@Component({
  selector: 'app-blog-posts-edit',
  templateUrl: './blog-posts-editing.component.html',
  styleUrls: ['./blog-posts-editing.component.css'],
  standalone: false,
})
export class BlogEditComponent implements OnInit {
  blogForm: FormGroup;
  blogId: number;
  selectedFile: File | null = null;
  imageUrl: string | null = null;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<BlogEditComponent>,
    private blogGetByIdService: BlogGetByIdForAdministrationService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private blogUpdateService: BlogUpdateOrInsertEndpointService,
  ) {
    this.blogId = 0;

    this.blogForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      content: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(1000)]],
      isPublished: [false, [Validators.required]],
      active: [false, [Validators.required]],
      image: [null] // Add image field to the form group
    });
  }

  ngOnInit(): void {
    this.blogId = this.data.blogId;  // Access the blogId passed to the dialog
    if (this.blogId !== 0) {
      this.loadBlogData();
    }
  }

  loadBlogData(): void {
    this.blogGetByIdService.handleAsync(this.blogId).subscribe({
      next: (blog) => {
        this.blogForm.patchValue({
          title: blog.title,
          content: blog.content,
          isPublished: blog.isPublished ?? false,
          active: blog.active ?? false,
        });

        // Set image field for display
        this.imageUrl = `data:image/jpeg;base64,${blog.image}`; // Set image URL to the new variable
      },
      error: (error) => console.error('Error loading blog data', error),
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input?.files?.length) {
      this.selectedFile = input.files[0];

      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.imageUrl = e.target.result;  // Save the image URL from FileReader
      };

      reader.readAsDataURL(this.selectedFile);

    }
  }

  save(): void {

    const blogPostData: BlogPostUpdateOrInsertRequest = this.blogForm.value;

    const formData = new FormData();
    formData.append('title', blogPostData.title);
    formData.append('content', blogPostData.content);
    formData.append('author', blogPostData.author);
    formData.append('isPublished', blogPostData.isPublished.toString());
    formData.append('active', blogPostData.active.toString());

    // Dodaj fajl ako je odabran
    if (this.selectedFile) {
      formData.append('image', this.selectedFile);
    }

    // Ako postoji ID, znači da je ažuriranje
    if (this.blogId !== 0) {
      formData.append('id', this.blogId.toString());
    }

    this.blogUpdateService.handleAsync(formData).subscribe({
      next: (response: any) => this.dialogRef.close('saved'),
      error: (error: any) => console.error('Greška pri spremanju!', error),
    });
  }

  close(): void {
    this.dialogRef.close(); // Simply close the modal
  }
}
