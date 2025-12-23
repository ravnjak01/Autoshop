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
  selectedProduct: ProductDTO | null = null;
  productForm!: FormGroup;
  isEditing: boolean = false;
  loading: boolean = false;
  availableImages: string[] = [];

  constructor(
    private productService: ProductService,
    private fb: FormBuilder,
    private snackBar:MySnackbarHelperService,
    private dialog:MatDialog
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadProducts();
    this.loadAvailableImages();
  }
  loadAvailableImages() {
  this.productService.GetAllImages().subscribe({
    next:(res)=>{
      this.availableImages = res.map(name =>
  `http://localhost:7000/images/products/${name}`
);

    },
    error:(err)=>console.error('Greška pri učitavanju slika:', err)
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
  this.productForm.patchValue({
    imageUrl:img
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
    this.productService.getAllProducts().subscribe({
      next: (res: ProductDTO[]) => {
        this.products = res.map((product: ProductDTO) => {
          if(product.imageUrl && !product.imageUrl.startsWith('http')){
            product.imageUrl=`http://localhost:7000/images/products/${product.imageUrl}`;
          }
          return product;
        });
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      }
    });
  }


  addProduct(): void {
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

    this.productService.addProduct(newProduct).subscribe({
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
    this.productForm.patchValue(product);
  }


  updateProduct(): void {
    if (this.productForm.invalid || !this.selectedProduct) {
      console.warn('Form not valid or no product selected for update!');
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

    this.productService.updateProduct(updatedProduct.id, updatedProduct).subscribe({
      next: () => {
this.snackBar.showMessage('Changes successfully saved!','success')
        this.isEditing = false;
        this.selectedProduct = null;
        this.loadProducts();
        this.productForm.reset({ active: true });
      },
      error: (err) => {
        console.error('error during updating the product', err);
      }
    });
  }


  deleteProduct(id: number): void {
    const dialogRef=this.dialog.open(MyDialogConfirmComponent,{

      width:'350px',
      data:{
        title:'Are you sure',
        message:'Do you really want to delete this product?',
        confirmButtonText: 'Delete'
      }
    });
    dialogRef.afterClosed().subscribe(result=>{
      if(result)
      {
  this.productService.deleteProduct(id).subscribe({
      next: () => {
       
        this.loadProducts();
      },
      error: (err) => {
        
      }
    });
      }
    });
  
  }
}
