import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Category, Product, ProductGetAllRequest, ProductGetAllResponse, ProductsGetAllService } from '../services/product-endpoints/product-get-all-endpoint.service';
import { CategoryGetAllService } from '../services/category-endpoints/category-get-all-endpoint.service';
import { Options } from '@angular-slider/ngx-slider';
import { CartService } from '../../cart/services/cart.service';
import { FormsModule, ReactiveFormsModule,FormBuilder,FormGroup,Validators } from '@angular/forms';
import { NgxSliderModule  } from '@angular-slider/ngx-slider';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ProductService } from '../services/product.service';
import { MyAuthService } from '../../core/services/auth/my-auth.service'
import { MySnackbarHelperService } from '../../modules/shared/snackbars/my-snackbar-helper.service';
import {FavoriteToggleEndpointService} from '../services/product-endpoints/favorites-toggle-endpoint.service';

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
    RouterModule
  ]
})
export class ProductsComponent implements OnInit {
  searchQuery: string = '';
  sortBy: string = 'createdDateDesc';
  selectedCategoryIds: number[] = [];
  categories: Category[] = [];
  products: Product[] = [];
  minPrice: number = 0;
  maxPrice: number = 1500;
  editing = false;
  isAdmin = false;
  successMessage: string | null = null;

  filterForm!: FormGroup;
  sliderOptions: Options = {
    floor: 0,
    ceil: 1000,
    step: 10,
    translate: (value: number): string => {
      return `${value} KM`;
    }
  };


  constructor(
    private fb: FormBuilder,
    private productsGetAllService: ProductsGetAllService,
    private categoriesGetAllService: CategoryGetAllService,
    private cartService: CartService,
    private productService: ProductService,
    public authService: MyAuthService,
    private snackbar: MySnackbarHelperService,
    private favoriteToggleService: FavoriteToggleEndpointService,
  ) {
  }

  productForm!: FormGroup;

  ngOnInit(): void {
    this.isAdmin = this.authService.isAdmin();
    this.productForm = this.fb.group({
      name: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(0.01)]],
      description: ['']
    });
    this.filterForm = this.fb.group({
      searchQuery: [''],
      categoryId: [null],
      minPrice: [0, Validators.min(0)],
      maxPrice: [1000, Validators.min(0)],
      sortBy: ['createdDateDesc'],
      stockQuantity: [false]
    });

    this.loadCategories();
    this.loadProducts();

  }

  editProduct(): void {
    this.editing = true;
  }

  cancelEdit(): void {
    this.editing = false;
  }

  onSubmit(): void {
    if (this.filterForm.invalid) {
      alert('Molimo ispravite greške u formi prije pretrage.');
      return;
    }

    const {minPrice, maxPrice} = this.filterForm.value;


    if (minPrice > maxPrice) {
      alert('Minimal cannot be higher that maximum.');
      return;
    }

    const {categoryId} = this.filterForm.value;
    let categoryIds: number[] = [];
    if (categoryId && categoryId > 0) {
      categoryIds = [categoryId];
    }


    const filterRequest: ProductGetAllRequest = {
      searchQuery: this.filterForm.value.searchQuery,
      categoryIds: categoryIds.length > 0 ? categoryIds : undefined,
      minPrice: this.filterForm.value.minPrice,
      maxPrice: this.filterForm.value.maxPrice,
      sortBy: this.filterForm.value.sortBy,
      pageNumber: 1,
      pageSize: 50,
      stockQuantity: this.filterForm.value.stockQuantity
    };


    this.productsGetAllService.handleAsync(filterRequest).subscribe({
      next: (response) => {
        this.products = response.products;

      },
      error: (err) => {
        console.error('Error during filtering:', err);
        alert('Error occured during the process of filtering.');
      }
    });
  }

  addToCart(productId: number): void {
    this.cartService.addToCart(productId, 1).subscribe({
      error: (err) => {
        this.snackbar.showMessage('Unfortunately ,this product isnt awailable in the wanted quantity.')
      }
    });
  }

  loadCategories(): void {
    this.categoriesGetAllService.handleAsync().subscribe((response) => {
      this.categories = response.categories;
    });
  }

  loadProducts(): void {
    if (this.filterForm.invalid) return;

    const {categoryId} = this.filterForm.value;
    let categoryIds: number[] = [];
    if (categoryId && categoryId > 0) {
      categoryIds = [categoryId];
    }
    const request: ProductGetAllRequest = {
      searchQuery: this.filterForm.value.searchQuery,
      categoryIds: this.filterForm.value.categoryId ? [this.filterForm.value.categoryId] : [],
      minPrice: this.filterForm.value.minPrice,
      maxPrice: this.filterForm.value.maxPrice,
      sortBy: this.filterForm.value.sortBy,
      pageNumber: 1,
      pageSize: 50
    };


    this.productsGetAllService.handleAsync(request).subscribe({
      next: (response: ProductGetAllResponse) => {
        this.products = response.products;
      },
      error: (err) => console.error('Error loading products', err)
    });
  }

  toggleFavorite(product: any) {
    this.favoriteToggleService.handleAsync(product.id)
      .subscribe(result => {
        product.isFavorite = result;
      });
  }
}
