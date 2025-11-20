import {Component, Inject, OnInit} from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  DiscountGetByIdForAdministrationService
} from '../../../../endpoints/discount-endpoints/discount-get-id-for-administration.service';
import {
  DiscountPostUpdateOrInsertRequest,
  DiscountUpdateOrInsertEndpointService
} from '../../../../endpoints/discount-endpoints/discount-add-edit-administration-endpoint.service';

function endDateAfterStartDate(control: AbstractControl): ValidationErrors | null {
  const start = control.get('startDate')?.value;
  const end = control.get('endDate')?.value;
  if (start && end && new Date(end) < new Date(start)) {
    return { endBeforeStart: true };
  }
  return null;
}

@Component({
  selector: 'app-discount-posts-edit',
  templateUrl: './discount-posts-editing.component.html',
  styleUrls: ['./discount-posts-editing.component.css'],
  standalone: false,
  
})
export class DiscountEditComponent implements OnInit {
  discountForm: FormGroup;
  discountId: number;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<DiscountEditComponent>,
    private discountGetByIdService: DiscountGetByIdForAdministrationService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private discountUpdateService: DiscountUpdateOrInsertEndpointService,
  ) {
    this.discountId = 0;

    this.discountForm = this.fb.group({
        name: ['',[Validators.required, Validators.minLength(2), Validators.maxLength(100)],],
        discountPercentage: ['', [Validators.required, Validators.min(0), Validators.max(100)],],
        startDate: [null, Validators.required],
        endDate: [null, Validators.required],
      },
      { validators: endDateAfterStartDate }
    );
  }

  ngOnInit(): void {
    this.discountId = this.data.discountId;  // Access the blogId passed to the dialog
    if (this.discountId !== 0) {
      this.loadDiscountData();
    }
  }

  loadDiscountData(): void {
    this.discountGetByIdService.handleAsync(this.discountId).subscribe({
      next: (discount) => {
        this.discountForm.patchValue({
          name: discount.name,
          discountPercentage: discount.discountPercentage,
          startDate: discount.startDate ? new Date(discount.startDate) : '',
          endDate: discount.endDate ? new Date(discount.endDate) : ''
        });
      },
      error: (error) => console.error('Error loading discount data', error),
    });
  }

  save(): void {
    if (this.discountForm.invalid) {
      this.discountForm.markAllAsTouched();
      return;
    }

    const discountPostData: DiscountPostUpdateOrInsertRequest = this.discountForm.value;

    const formData = new FormData();
    formData.append('name', discountPostData.name);
    formData.append('discountPercentage', discountPostData.discountPercentage.toString());
    formData.append('startDate', (discountPostData.startDate as unknown as Date).toISOString());
    formData.append('endDate', (discountPostData.endDate as unknown as Date).toISOString());

    if (this.discountId && this.discountId !== 0) {
      formData.append('id', this.discountId.toString());
      this.discountUpdateService.handleAsync(formData).subscribe({
        next: () => this.dialogRef.close('updated'),
        error: (error: any) => console.error('Greška pri ažuriranju popusta!', error),
      });
    } else {
      this.discountUpdateService.handleAsync(formData).subscribe({
        next: () => this.dialogRef.close('saved'),
        error: (error: any) => console.error('Greška pri spremanju popusta!', error),
      });
    }
  }

  close(): void {
    this.dialogRef.close();
  }
}
