import {Component} from '@angular/core';
import {MatDialog} from '@angular/material/dialog';
import {BlogEditComponent} from '../blog-posts/blog-posts-editing/blog-posts-editing.component';
import {DiscountEditComponent} from '../discount/discount-post-editing/discount-posts-editing.component';

@Component({
  selector: 'app-administration-root',
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css',
  standalone: false
})
export class HomePageComponent {
  title = 'Administration ';
  constructor(
    private dialog: MatDialog
  ) {
  }
  addBlog() {
    const dialogRef = this.dialog.open(BlogEditComponent, {
      width: '800px', // Povećana širina dijaloga
      maxHeight: '80vh', // Maksimalna visina dijaloga
      data: { blogId: 0 }, // Pass blogId if editing, 0 for new blog
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
