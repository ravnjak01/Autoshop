import { UserRole } from "../../../core/services/auth/my-auth.service";

// DTO to hold authentication information
export interface MyAuthInfo {

  username: string;
  roles:UserRole [];
}
