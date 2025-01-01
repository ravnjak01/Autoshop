import {Component, Inject, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {
  BlogUpdateOrInsertEndpointService,
  BlogPostUpdateOrInsertRequest
} from '../../../../endpoints/blog-endpoints/blogs-add-edit-administration-endpoint.service';
//import {CityGetByIdEndpointService} from '../../../../endpoints/city-endpoints/city-get-by-id-endpoint.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

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

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<BlogEditComponent>,
    //private cityGetByIdService: CityGetByIdEndpointService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private blogUpdateService: BlogUpdateOrInsertEndpointService,
  ) {
    this.blogId = 0;

    this.blogForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(30)]],
      content: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(1000)]],
      isPublished: [false, [Validators.required]],
    });
  }

  ngOnInit(): void {
    // this.blogId = Number(this.route.snapshot.paramMap.get('id'));
    // if (this.blogId) {
    //   this.loadBlogData();
    // }
  }

  // loadBlogData(): void {
  //   this.cityGetByIdService.handleAsync(this.cityId).subscribe({
  //     next: (city) => {
  //       this.cityForm.patchValue({
  //         name: city.name,
  //         countryId: city.countryId,
  //         regionId: city.regionId,
  //       });
  //       this.loadRegionsForCountry(city.countryId); // Preload regions for the selected country
  //     },
  //     error: (error) => console.error('Error loading city data', error),
  // }
  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input?.files?.length) {
      this.selectedFile = input.files[0];
    }
  }

  save(): void {

    const blogPostData: BlogPostUpdateOrInsertRequest = this.blogForm.value;

    const formData = new FormData();
    formData.append('title', blogPostData.title);
    formData.append('content', blogPostData.content);
    formData.append('author', blogPostData.author);
    formData.append('isPublished', blogPostData.isPublished.toString());

    // Dodaj fajl ako je odabran
    if (this.selectedFile) {
      formData.append('image', this.selectedFile);
    }

    // Ako postoji ID, znači da je ažuriranje
    if (blogPostData.id) {
      formData.append('id', blogPostData.id.toString());
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
