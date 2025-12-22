import { Routes } from '@angular/router';
import { AdministrationComponent } from './administration/components/administration.component';
import { HomePageComponentAdministration } from './administration/home/components/home-page-component-administration.component';
import { RegisterComponent } from './auth/register/register.component';
import { LoginComponent } from './auth/login/login.component';
import { ForgotPasswordComponent } from './auth/forgot-password/forgot-password';
import { ResetPasswordComponent } from './auth/forgot-password/reset-password/reset-password.component';
import { AuthGuard } from './core/guards/auth/auth.guard';
import {  CartPageComponent } from './cart/components/cart-page/cart-page.component';
import { CartSidebarComponent } from './cart/components/cart-sidebar/cart-sidebar.component';
import { CheckoutComponent } from './checkout/checkout/checkout.component';
import { ConfirmationModalComponent } from './confirmation-modal/confirmation-modal/confirmation-modal.component';
import { ProductManagementComponent } from './administration/products/product-management/product-management.component';
import { CategoriesComponent } from './categories/components/categories.component';
import { DiscountsComponent } from './administration/discount-management/components/discount/discount.component';
import {BlogPostsComponent as BlogPostsComponentsAdministration} from './administration/blog-management/components/blogs/blog-posts.component' ;
import {BlogPostsComponent} from './blog/components/blog-posts/blog-posts.component';
import {BlogDetailsComponent} from './blog/components/blog-post/blog-post.component';
import {HomePageComponent} from './home/components/home-page.component';

export const appRoutes: Routes = [
  {
    path: 'administration',
    component: AdministrationComponent,
       canActivate: [AuthGuard],
  data: { isAdmin: true },
    children: [
           //{ path: '', redirectTo: 'admin/home-page', pathMatch: 'full' },
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      { path: 'home', component: HomePageComponentAdministration },
      {    path: 'product-management',
  component: ProductManagementComponent},
       { path: 'admin/blog-posts', component: BlogPostsComponentsAdministration },
         { path: 'admin/discount', component: DiscountsComponent},
         { path: 'admin/home-page', component: HomePageComponentAdministration },

    ]
  },
    { path: 'home', component: HomePageComponent },
{
      path: 'products',
    loadComponent: () =>
      import('./products/components/products.component').then(c => c.ProductsComponent)
  },
  {path: 'blogs', component: BlogPostsComponent},
  { path: 'blog/:id', component: BlogDetailsComponent },
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
{path:'categories',component:CategoriesComponent},
    { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: '**', redirectTo: 'home' }
];
