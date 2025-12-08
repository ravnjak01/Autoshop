import { Component, Inject, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  DiscountCodePostRequest,
  DiscountCodeUpdateService
} from '../../../services/discount-code-add-edit-administration-endpoint.service';

@Component({
  selector: 'app-discount-code-edit',
  templateUrl: './discount-code-edit.component.html',
  styleUrls: ['./discount-code-edit.component.css'],
  standalone: false
})
export class DiscountCodeEditComponent implements OnInit {
  codeForm: FormGroup;
  codeId?: number;

  constructor(
    private fb: FormBuilder,
    private discountService: DiscountCodeUpdateService,
    private dialogRef: MatDialogRef<DiscountCodeEditComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { code?: any; discountId?: number }
  ) {
    this.codeForm = this.fb.group({
      id: [null],
      code: ['', Validators.required],
      discountId: [0, Validators.required],
      validFrom: [new Date(), Validators.required],
      validTo: [new Date(), Validators.required]
    });
  }

  ngOnInit(): void {
    if (this.data?.code && this.data.code.id) {
      // ✅ EDIT mod
      const code = this.data.code;
      this.codeId = code.id;

      this.codeForm.patchValue({
        id: code.id,
        code: code.code,
        discountId: this.data.discountId ?? code.discountId, // uvijek šalji discountId
        validFrom: code.validFrom ? new Date(code.validFrom) : new Date(),
        validTo: code.validTo ? new Date(code.validTo) : new Date()
      });
    } else {
      // ✅ ADD mod
      this.codeId = undefined;
      this.codeForm.patchValue({
        discountId: this.data.discountId ?? 0 // discountId dolazi iz parenta
      });
    }
  }

  save(): void {
    const raw: DiscountCodePostRequest = this.codeForm.value;

    const formData = new FormData();

    if (raw.id) formData.append('id', raw.id.toString());
    formData.append('code', raw.code || '');
    formData.append('discountId', (raw.discountId ?? this.data?.discountId ?? 0).toString());
    formData.append('validFrom', new Date(raw.validFrom).toISOString());
    formData.append('validTo', new Date(raw.validTo).toISOString());

    this.discountService.handleAsync(formData).subscribe({
      next: () => this.dialogRef.close('updated'),
      error: err => console.error('Error saving code', err)
    });
  }

  close(): void {
    this.dialogRef.close();
  }
}
