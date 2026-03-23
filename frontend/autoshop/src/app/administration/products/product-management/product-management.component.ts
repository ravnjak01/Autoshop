import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductCreateDTO, ProductDTO, ProductUpdateDTO } from '../../../cart/models/product.dto';
import { ProductService } from '../../../products/services/product.service';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {MySnackbarHelperService} from '../../../modules/shared/snackbars/my-snackbar-helper.service'
import { MatDialog } from '@angular/material/dialog';
import { MyDialogConfirmComponent } from '../../../modules/shared/dialogs/my-dialog-confirm/my-dialog-confirm.component';
import { title } from 'process';
import { Product, ProductGetAllRequest, ProductGetAllResponse, ProductsGetAllService } from '../../../products/services/product-endpoints/product-get-all-endpoint.service';
import { HttpErrorResponse } from '@angular/common/http';
import { MyConfig } from '../../../my-config';

@Component({
  selector: 'app-product-management',
  templateUrl: './product-management.component.html',
  styleUrls: ['./product-management.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule]
})
export class ProductManagementComponent implements OnInit {
cancelEdit(): void {
  if (this.selectedProduct) {
    this.productForm.patchValue(this.selectedProduct);
  } else {
    this.productForm.reset();
  }
  this.isEditing = false;
  this.selectedProduct = null;
}


  products: ProductDTO[] = [];
  public MyConfig = MyConfig;
  readonly API_BASE_URL = `${MyConfig.api_address}`;
  selectedProduct: ProductDTO | null = null;
  productForm!: FormGroup;
  isEditing: boolean = false;
  loading: boolean = false;
  availableImages: string[] = [];
  currentPage: number = 1;
  pageSize: number = 5;
  totalItems: number = 0; 
  totalPages: number = 0;
  submitted: boolean = false;
  constructor(
    private productCRUDService: ProductService,
    private fb: FormBuilder,
    private snackBar:MySnackbarHelperService,
    private dialog:MatDialog,
    private productGetAllService:ProductsGetAllService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadProducts();
    this.loadAvailableImages();
  }

handleMissingImage(product: ProductDTO) {
  product.imageUrl = `${MyConfig.api_address}${MyConfig.ImagesPath}no-image.png`;
}
  loadAvailableImages() {
  this.productCRUDService.GetAllImages().subscribe({
    next: (res) => {
      this.availableImages = res; 
    },
  });
}

showImageModal = false;

openImageModal() {
  this.showImageModal = true;
}

closeImageModal() {
  this.showImageModal = false;
}

selectImage(img: string) {
  const fileName = img.split(/[\\/]/).pop() || '';

  const relativePath = `/images/products/${fileName}`;

  this.productForm.patchValue({
    imageUrl: relativePath
  });

  this.showImageModal = false;
}


  initForm(): void {
    this.productForm = this.fb.group({
      id: [0],
      name: ['', [Validators.required, Validators.minLength(2)]],
      sku: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(0.01)]],
      stockQuantity: [0, [Validators.required, Validators.min(0)]],
      description: [''],
      imageUrl: ['', Validators.required],
      categoryId: [null, Validators.required],
      brend: ['', Validators.required],
      active: [true]
    });
  }




  loadProducts(): void {
    this.loading = true;

    const request:ProductGetAllRequest={
      pageNumber: this.currentPage, 
    pageSize: this.pageSize,
    }
    this.productGetAllService.handleAsync(request).subscribe({
      next: (res: ProductGetAllResponse) => {
       this.products = res.products.map((product: ProductDTO) => {
      if (!product.imageUrl || product.imageUrl.includes('${')) {
        product.imageUrl = `${MyConfig.api_address}${MyConfig.ImagesPath}no-image.png`;
      } else if (!product.imageUrl.startsWith('http')) {
        product.imageUrl = `${MyConfig.api_address}${MyConfig.ImagesPath}${product.imageUrl}`;
      }
      return product;
});
        this.loading = false;
      },
      error: (err:HttpErrorResponse) => {
        this.loading = false;
      }
    });
  }
  changePage(delta: number): void {
    const newPage = this.currentPage + delta;
    if (newPage > 0) {
      this.currentPage = newPage;
      this.loadProducts();
    }
  }
onPageSizeChange(event: Event): void {
  const selectElement = event.target as HTMLSelectElement;
  this.pageSize = Number(selectElement.value);
  this.currentPage = 1; 
  this.loadProducts();
}
  addProduct(): void {
    this.submitted = true;
    if (this.productForm.invalid) {
     
      return;
    }

    const newProduct: ProductCreateDTO = {
      id: 0,
      name: this.productForm.value.name,
      sku: this.productForm.value.sku,
      price: this.productForm.value.price,
      stockQuantity: this.productForm.value.stockQuantity,
      description: this.productForm.value.description,
      imageUrl: this.productForm.value.imageUrl,
      categoryId: this.productForm.value.categoryId,
      brend: this.productForm.value.brend,
      active: this.productForm.value.active
    };

    this.productCRUDService.addProduct(newProduct).subscribe({
      next: () => {
        this.snackBar.showMessage('Product added!','success');
        this.loadProducts();
        this.productForm.reset({ active: true }); 
      },
      error: (err) => {
  this.snackBar.showMessage('Form not correctly filled','error')      }
    });
  }


  editProduct(product: ProductDTO): void {
    this.isEditing = true;
    this.selectedProduct = product;

     const relativeImageUrl = product.imageUrl?.startsWith('http')
    ? product.imageUrl.replace(MyConfig.api_address, '')
    : product.imageUrl;


     this.productForm.patchValue({
    ...product,
    imageUrl: relativeImageUrl  
  });
  }


  updateProduct(): void {
    this.submitted = true;
    if (this.productForm.invalid || !this.selectedProduct) {
      return;
    }

    const updatedProduct: ProductUpdateDTO = {
      id: this.selectedProduct.id,
      name: this.productForm.value.name,
      price: this.productForm.value.price,
      imageUrl: this.productForm.value.imageUrl,
      brend: this.productForm.value.brend,
      stockQuantity: this.productForm.value.stockQuantity,
      description: this.productForm.value.description,
      active: this.productForm.value.active,
      sku: this.productForm.value.sku,
      categoryId: this.productForm.value.categoryId
    };

    this.productCRUDService.updateProduct(updatedProduct.id, updatedProduct).subscribe({
      next: () => {
this.snackBar.showMessage('Changes successfully saved!','success')
        this.isEditing = false;
        this.selectedProduct = null;
        this.loadProducts();
        this.productForm.reset({ active: true });
      },
      error: (err) => {
      }
    });
  }
get f() { return this.productForm.controls; }
deleteProduct(id: number): void {
  const dialogRef = this.dialog.open(MyDialogConfirmComponent, {
    width: '350px',
    data: {
      title: 'Are you sure',
      message: 'Do you really want to delete this product?',
      confirmButtonText: 'Delete'
    }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      this.productCRUDService.deleteProduct(id).subscribe({
        next: () => {
          this.snackBar.showMessage('Product successfully deleted!', 'success');
          this.loadProducts();
        },
        error: (err: HttpErrorResponse) => {
          const errorMessage = err.error?.message || err.error || 'Error during deleting the product';
          this.snackBar.showMessage(errorMessage, 'error');
        }
      });
    }
  });
}
}
