import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { DiscountCode, DiscountGetCodesService } from '../../services/discount-codes-get-all-endpoint.service';
import { DiscountCodeDeleteEndpointService } from '../../services/discount-code-delete-administration.service';
import { DiscountCodeEditComponent } from './discount-code-edit/discount-code-edit.component';

@Component({
  selector: 'app-discount-codes',
  templateUrl: 'discount-codes.component.html',
  styleUrls: ['./discount-codes.component.css'],
  standalone: false
})
export class DiscountCodesComponent implements OnInit {
  codes: DiscountCode[] = [];
  displayedColumns: string[] = ['code', 'validFrom', 'validTo', 'actions'];

  constructor(
    private dialogRef: MatDialogRef<DiscountCodesComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { discountId: number },
    private discountGetCodesService: DiscountGetCodesService,
    private discountCodeDeleteService: DiscountCodeDeleteEndpointService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadCodes();
  }

  loadCodes(): void {
    this.discountGetCodesService.handleAsync(this.data.discountId).subscribe({
      next: codes => this.codes = codes,
      error: err => console.error('Error loading codes', err)
    });
  }

  editCode(code: DiscountCode): void {
    const dialogRef = this.dialog.open(DiscountCodeEditComponent, {
      width: '400px',
      data: {
        discountId: this.data.discountId, // uvijek mora biti tu
        code: code                   // dodatno šaljemo code kada je edit
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 'updated') {
        this.loadCodes();
      }
    });
  }

  addCode(): void {
    const dialogRef = this.dialog.open(DiscountCodeEditComponent, {
      width: '400px',
      data: { discountId: this.data.discountId } // za add
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 'updated') {
        this.loadCodes();
      }
    });
  }

  deleteCode(id: number, event: MouseEvent): void {
    event.stopPropagation();
    if (confirm('Are you sure you want to delete this code?')) {
      this.discountCodeDeleteService.handleAsync(id).subscribe({
        next: () => this.loadCodes(),
        error: err => console.error('Error deleting code', err)
      });
    }
  }

  close(): void {
    this.dialogRef.close('updated');
  }
}
