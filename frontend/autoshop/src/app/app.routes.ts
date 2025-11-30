import { Routes } from '@angular/router';
import { AdministrationComponent } from './administration/components/administration.component';
import { HomePageComponent } from './home/components/home-page.component';
import { RegisterComponent } from './auth/register/register.component';
import { LoginComponent } from './auth/login/login.component';
import { ForgotPasswordComponent } from './auth/forgot-password/forgot-password';
import { ResetPasswordComponent } from './auth/forgot-password/reset-password/reset-password.component';
import { AuthGuard } from './core/guards/auth/auth.guard';
import {  CartPageComponent } from './cart/components/cart-page/cart-page.component';
import { CartSidebarComponent } from './cart/components/cart-sidebar/cart-sidebar.component';
import { CheckoutComponent } from './checkout/checkout/checkout.component';
import { ConfirmationModalComponent } from './confirmation-modal/confirmation-modal/confirmation-modal.component';
import { BlogPostsComponent } from './blog/components/blog-posts/blog-posts.component';
import { DiscountsComponent } from './modules/administration/discount/discount.component';
export const appRoutes: Routes = [
  {
    path: 'administration',
    component: AdministrationComponent,
    children: [
           //{ path: '', redirectTo: 'admin/home-page', pathMatch: 'full' },
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      { path: 'home', component: HomePageComponent },
     { path: 'admin/blog-posts', component: BlogPostsComponent },
         { path: 'admin/discount', component: DiscountsComponent},
         { path: 'admin/home-page', component: HomePageComponent },
    ]
  },
    { path: 'home', component: HomePageComponent },

  {
    path: 'products',
    loadComponent: () =>
      import('./products/components/products.component').then(c => c.ProductsComponent)
  },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  { path: 'cart', component: CartPageComponent },
  { path: 'cartSide', component: CartSidebarComponent },
  
  {
  path: 'checkout',
  component: CheckoutComponent,
  canActivate: [AuthGuard]
},
{path:'confirmation-modal',component:ConfirmationModalComponent},
    { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: '**', redirectTo: 'home' }
];
