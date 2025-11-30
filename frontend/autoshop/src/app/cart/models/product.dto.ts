import { Category } from "../../products/services/product-endpoints/product-get-all-endpoint.service";



export interface ProductDTO {
id: number;
  name: string;
  price: number; 
  imageUrl: string;
  

  categoryId: number;
  category: Category; 
  
 
  stockQuantity?: number; 
  description?: string; 
  sku?: string;
  brend: string; 
  code?: string;
  

  additionalImagesUrl?: string[]; 
  

  avgGrade?: number; 
  numberOfReviews?: number; 
  

  active: boolean;
  createdAt?: Date; 

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
