import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import {
  DiscountCategories,
  DiscountCategoryGetAllService
} from '../../services/discount-categories-get-all-endpoint.service';
import {
  DiscountCategoryRequest,
  DiscountCategorySaveService
} from '../../services/discount-categories-save-endpoint.service';

@Component({
  selector: 'app-discount-category-dialog',
  templateUrl: './discount-category.component.html',
  styleUrls: ['./discount-category.component.css'],
  standalone: false
})
export class DiscountCategoryDialogComponent implements OnInit {
  categories: DiscountCategories[] = [];
  selectedCategoryIds: number[] = [];
  filterText: string = '';
  checkAllToggle: boolean = false; // ✅ check all toggle

  constructor(
    private dialogRef: MatDialogRef<DiscountCategoryDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { discountId: number, discountName: string },
    private discountCategoryGetAllService: DiscountCategoryGetAllService,
    private discountCategorySaveService: DiscountCategorySaveService
  ) {}

  ngOnInit(): void {
    this.discountCategoryGetAllService.handleAsync(this.data.discountId)
      .subscribe(categories => {
        this.categories = categories;
        this.selectedCategoryIds = categories.filter(c => c.isSelected).map(c => c.categoryId);
        this.updateCheckAllToggle();
      });
  }

  filteredCategories(): DiscountCategories[] {
    return this.categories.filter(c =>
      c.categoryName.toLowerCase().includes(this.filterText.toLowerCase())
    );
  }

  toggleCategory(id: number): void {
    const category = this.categories.find(c => c.categoryId === id);
    if (!category) return;

    category.isSelected = !category.isSelected;

    const index = this.selectedCategoryIds.indexOf(id);
    if (index > -1) {
      this.selectedCategoryIds.splice(index, 1);
    } else {
      this.selectedCategoryIds.push(id);
    }

    this.updateCheckAllToggle();
  }

  toggleCheckAll(): void {
    if (this.checkAllToggle) {
      this.filteredCategories().forEach(c => {
        c.isSelected = true;
        if (!this.selectedCategoryIds.includes(c.categoryId)) {
          this.selectedCategoryIds.push(c.categoryId);
        }
      });
    } else {
      this.filteredCategories().forEach(c => {
        c.isSelected = false;
        const index = this.selectedCategoryIds.indexOf(c.categoryId);
        if (index > -1) this.selectedCategoryIds.splice(index, 1);
      });
    }
  }

  private updateCheckAllToggle(): void {
    const filtered = this.filteredCategories();
    this.checkAllToggle = filtered.length > 0 && filtered.every(c => c.isSelected);
  }

  save(): void {
    const request: DiscountCategoryRequest = {
      discountId: this.data.discountId,
      categoryIds: this.selectedCategoryIds
    };
    this.discountCategorySaveService.handleAsync(request)
      .subscribe(() => this.dialogRef.close(true));
  }

  close(): void {
    this.dialogRef.close();
  }
}
