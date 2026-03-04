import { Category } from "../../products/services/product-endpoints/product-get-all-endpoint.service";



export interface ProductDTO {
  id: number;
  name: string;
  price: number;
  description?: string; 
  imageUrl?: string;
  sku?: string;
  brend?: string;
  code?: string;
  stockQuantity: number;
  active: boolean;
  categoryId: number;
  categoryName?: string; 
  
  
  priceAfterGlobalDiscount?: number;
  badgeDiscountPercentage?: number;
  isFavorite: boolean;

  additionalImagesUrl?: string[]; 

  createdAt?: Date | string; 
}

export interface ProductCreateDTO {
  id: number;
  name: string;
  description?: string;
  imageUrl: string;
  price: number;
  categoryId: number;
  sku: string;
  stockQuantity: number;
  brend: string;
  active: boolean;

}

export interface ProductUpdateDTO {

id: number;

  name?: string;
  price?: number; 
  
  imageUrl?: string;
  brend?: string; 
  
  stockQuantity?: number; 
  description?: string;
  active?: boolean;
  sku?: string;
  categoryId?: number;
  

  additionalImagesUrl?: string[];
}
