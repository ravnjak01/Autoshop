
import {Component} from '@angular/core';
import {MatDialog, MatDialogModule} from '@angular/material/dialog';
import {BlogEditComponent} from '../../blog-management/components/blog-posts-editing/blog-posts-editing.component';
import { DiscountEditComponent } from '../../discount-management/components/discount-post-editing/discount-posts-editing.component';
@Component({
    selector: 'app-administration-root',
    templateUrl: './home-page-component-administration.component.html',
    styleUrl: './home-page-component-administration.component.css',
    standalone: true,
    imports: [
        MatDialogModule
    ]
})
export class HomePageComponentAdministration {

  title = 'Administration ';
  constructor(
    private dialog: MatDialog
  ) {
  }
  addBlog() {
    const dialogRef = this.dialog.open(BlogEditComponent, {
      width: '800px',
      maxHeight: '80vh',
      data: { blogId: 0 },
    });
  }
  addDiscount() {
    const dialogRef = this.dialog.open(DiscountEditComponent, {
      width: '800px', // Povećana širina dijaloga
      maxHeight: '80vh', // Maksimalna visina dijaloga
      data: { discountId: 0 },
    });
  }
}

