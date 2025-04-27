import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Category, Product, ProductGetAllRequest, ProductGetAllResponse, ProductsGetAllService } from '../../endpoints/product-endpoints/product-get-all-endpoint.service';
import { CategoryGetAllService } from '../../endpoints/category-endpoints/category-get-all-endpoint.service';
import { Options } from '@angular-slider/ngx-slider';

@Component({
  selector: 'app-product-list',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css'],
  encapsulation: ViewEncapsulation.None,
  standalone: false
})
export class ProductListComponent implements OnInit {
  searchQuery: string = '';
  sortBy: string = 'createdDateDesc'; // Default: Najnovije prvo
  selectedCategoryIds: number[] = [];
  categories: Category[] = [];
  products: Product[] = [];
  minPrice: number = 0;
  maxPrice: number = 1000;
  sliderOptions: Options = {
    floor: 0,
    ceil: 1000,
    step: 10,
    translate: (value: number): string => {
      return `${value} KM`;
    }
  };

  constructor(private productsGetAllService: ProductsGetAllService, private categoriesGetAllService: CategoryGetAllService) {}

  ngOnInit(): void {
    this.loadCategories();
    this.loadProducts();
  }

  // Load Categories (can be replaced with a real API call)
  loadCategories(): void {
    this.categoriesGetAllService.handleAsync().subscribe((response) => {
      this.categories = response.categories;
    });
  }

  // Fetch Products with current filters
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
    });
  }

  // Handle Search Input Change
  onSearchChange(): void {
    this.loadProducts();
  }

  // Handle Category Filter Change
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

  // Handle Sorting Change
  onSortChange(): void {
    this.loadProducts();
  }

  // Handle Category Filter Submit (optional, in case you want to trigger filters manually)
  onFilterSubmit(): void {
    this.loadProducts();
  }
}
