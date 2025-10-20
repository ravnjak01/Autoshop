
import {Component} from '@angular/core';
import {MatDialog, MatDialogModule} from '@angular/material/dialog';
import {BlogEditComponent} from '../../administration/blog-management/components/blog-posts-editing/blog-posts-editing.component';

@Component({
  selector: 'app-administration-root',
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css',
  standalone: true,
  imports: [
    MatDialogModule 
  ]
})
export class HomePageComponent {
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
}

