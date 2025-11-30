import { CartItemDTO } from "./cart-item.dto";

export interface CartResponseDTO {
  items: CartItemDTO[];
  total: number;
  itemCount: number;
}