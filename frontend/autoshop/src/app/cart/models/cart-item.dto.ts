import { ProductDTO } from "./product.dto";



export interface CartItemDTO {
   id: number;           
  productId: number;
  productName: string;
  price: number;
  quantity: number;
  total: number;
  product:ProductDTO
  imageUrl:string;
}




