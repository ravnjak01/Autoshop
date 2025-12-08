import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import {
  DiscountProductGetAllService,
  DiscountProducts
} from '../../services/discount-products-get-all-endpoint.service';
import {
  DiscountProductSaveService, DiscountProductsRequest
} from '../../services/discount-products-save-endpoint.service';

@Component({
  selector: 'app-discount-product-dialog',
  templateUrl: './discount-product.component.html',
  styleUrls: ['./discount-product.component.css'],
  standalone: false
})
export class DiscountProductDialogComponent implements OnInit {

  products: DiscountProducts[] = [];
  selectedProductIds: number[] = [];
  filterText: string = '';
  checkAllToggle: boolean = false;

  constructor(
    private dialogRef: MatDialogRef<DiscountProductDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { discountId: number, discountName: string },
    private discountProductGetAllService: DiscountProductGetAllService,
    private discountProductSaveService: DiscountProductSaveService
  ) {}

  ngOnInit(): void {
    this.discountProductGetAllService.handleAsync(this.data.discountId)
      .subscribe(products => {
        this.products = products;

        // Preload selected
        this.selectedProductIds = products
          .filter(p => p.isSelected)
          .map(p => p.productId);

        this.updateCheckAllToggle();
      });
  }

  filteredProducts(): DiscountProducts[] {
    const filter = this.filterText.toLowerCase();
    return this.products.filter(p =>
      p.productName.toLowerCase().includes(filter) ||
      p.productCode.toLowerCase().includes(filter)
    );
  }

  toggleProduct(product: DiscountProducts): void {
    const index = this.selectedProductIds.indexOf(product.productId);

    if (product.isSelected) {
      // Add if checked
      if (index < 0) this.selectedProductIds.push(product.productId);
    } else {
      // Remove if unchecked
      if (index > -1) this.selectedProductIds.splice(index, 1);
    }

    this.updateCheckAllToggle();
  }

  toggleCheckAll(): void {
    const filtered = this.filteredProducts();

    if (this.checkAllToggle) {
      // Select all
      filtered.forEach(p => {
        p.isSelected = true;
        if (!this.selectedProductIds.includes(p.productId)) {
          this.selectedProductIds.push(p.productId);
        }
      });
    } else {
      // Unselect all
      filtered.forEach(p => {
        p.isSelected = false;
        const index = this.selectedProductIds.indexOf(p.productId);
        if (index > -1) this.selectedProductIds.splice(index, 1);
      });
    }
  }

  private updateCheckAllToggle(): void {
    const filtered = this.filteredProducts();
    this.checkAllToggle = filtered.length > 0 && filtered.every(p => p.isSelected);
  }

  save(): void {
    const request: DiscountProductsRequest = {
      discountId: this.data.discountId,
      productIds: this.selectedProductIds
    };

    this.discountProductSaveService.handleAsync(request)
      .subscribe(() => this.dialogRef.close(true));
  }

  close(): void {
    this.dialogRef.close();
  }
}
