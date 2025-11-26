import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Category, Product, ProductGetAllRequest, ProductGetAllResponse, ProductsGetAllService } from '../services/product-endpoints/product-get-all-endpoint.service';
import { CategoryGetAllService } from '../services/category-endpoints/category-get-all-endpoint.service';
import { Options } from '@angular-slider/ngx-slider';
import { CartService } from '../../cart/services/cart.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSliderModule  } from '@angular-slider/ngx-slider';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../modules/shared/shared.module';
@Component({
  selector: 'app-product-list',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule, 
    ReactiveFormsModule, 
    NgxSliderModule, 
RouterModule]
})
export class ProductsComponent implements OnInit {
  searchQuery: string = '';
  sortBy: string = 'createdDateDesc'; 
  selectedCategoryIds: number[] = [];
  categories: Category[] = [];
  products: Product[] = [];
  minPrice: number = 0;
  maxPrice: number = 1500;
  successMessage: string | null = null;

  sliderOptions: Options = {
    floor: 0,
    ceil: 1000,
    step: 10,
    translate: (value: number): string => {
      return `${value} KM`;
    }
  };

  constructor(private productsGetAllService: ProductsGetAllService, private categoriesGetAllService: CategoryGetAllService,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
      console.log('ProductsComponent loaded!');
    this.loadCategories();
    this.loadProducts();
  }
 addToCart(productId: number): void {
  this.cartService.addToCart(productId, 1).subscribe({
    next: () => alert('Product added to cart!'),
    error: (err) => console.error('Error during adding the product', err)
  });
}
 
  loadCategories(): void {
    this.categoriesGetAllService.handleAsync().subscribe((response) => {
      this.categories = response.categories;
    });
  }


  loadProducts(): void {
    const request: ProductGetAllRequest = {
      searchQuery: this.searchQuery,
      categoryIds: this.selectedCategoryIds,
      minPrice: this.minPrice,
      maxPrice: this.maxPrice,
      sortBy: this.sortBy
    };

    this.productsGetAllService.handleAsync(request).subscribe((response: ProductGetAllResponse) => {
   
    this.products = response.products;
    console.log('Products loaded:', this.products); 
  }, (error) => {
    console.error('Error loading products', error);
  });
  }


  onSearchChange(): void {
    this.loadProducts();
  }


  onCategoryChange(event: Event, categoryId: number): void {
    const checkbox = event.target as HTMLInputElement;
    if (checkbox.checked) {
      this.selectedCategoryIds.push(categoryId);
    } else {
      const index = this.selectedCategoryIds.indexOf(categoryId);
      if (index > -1) {
        this.selectedCategoryIds.splice(index, 1);
      }
    }
    this.loadProducts();
  }

 
  onSortChange(): void {
    this.loadProducts();
  }

 
  onFilterSubmit(): void {
    this.loadProducts();
  }
}
