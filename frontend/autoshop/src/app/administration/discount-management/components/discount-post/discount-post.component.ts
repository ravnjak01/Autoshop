import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {Component, Inject, OnInit} from '@angular/core';
import {
  DiscountGetByIdForAdministrationService
} from '../../services/discount-get-id-for-administration.service';

@Component({
  selector: 'app-discount-post',
  templateUrl: './discount-post.component.html',
  styleUrls: ['./discount-post.component.css'],
  standalone: false,
})
export class DiscountPostComponent implements OnInit {
  discount: any = {};

  constructor(
    private dialogRef: MatDialogRef<DiscountPostComponent>,
    private discountGetByIdService: DiscountGetByIdForAdministrationService,
    @Inject(MAT_DIALOG_DATA) public data: { discountId: number }
  ) {}

  ngOnInit(): void {
    this.loadDiscountData();
  }

  loadDiscountData(): void {
    this.discountGetByIdService.handleAsync(this.data.discountId).subscribe({
      next: (discount) => {
        this.discount = discount;
      },
      error: (error) => console.error('Error loading blog data', error),
    });
  }

  close(): void {
    this.dialogRef.close(); // Close the dialog
  }
}
