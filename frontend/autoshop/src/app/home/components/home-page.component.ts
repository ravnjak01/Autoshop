
import { Component } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { BlogEditComponent } from '../../administration/blog-management/components/blog-posts-editing/blog-posts-editing.component';
import { DiscountEditComponent } from '../../modules/administration/discount/discount-post-editing/discount-posts-editing.component';
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
    addDiscount() {
        const dialogRef = this.dialog.open(DiscountEditComponent, {
            width: '800px', // Povećana širina dijaloga
            maxHeight: '80vh', // Maksimalna visina dijaloga
            data: { discountId: 0 },
        });
    }
}

