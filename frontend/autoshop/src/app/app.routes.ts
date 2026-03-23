import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth/auth.guard';

export const appRoutes: Routes = [

  { path: '', loadComponent: () => import('./home/components/home-page.component').then(c => c.HomePageComponent), pathMatch: 'full' },
  { path: 'home', loadComponent: () => import('./home/components/home-page.component').then(c => c.HomePageComponent) },

  {
    path: 'administration',
    loadComponent: () => import('./administration/components/administration.component').then(c => c.AdministrationComponent),
    canActivate: [AuthGuard],
    data: { isAdmin: true },
    children: [
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      {
        path: 'home',
        loadComponent: () => import('./administration/home/components/home-page-component-administration.component').then(c => c.HomePageComponentAdministration)
      },
      {
        path: 'product-management',
        loadComponent: () => import('./administration/products/product-management/product-management.component').then(c => c.ProductManagementComponent)
      },
     {
        path: 'blog-management', // localhost:4200/administration/blog-management
        loadChildren: () => import('./administration/blog-management/blog-management.module').then(m => m.BlogManagementModule)
      },
     {
        path: 'discount-management', 
        loadChildren: () => import('./administration/discount-management/discount-managements.module').then(m => m.DiscountModule)
      },
    ]
  },

  {
    path: 'products',
    loadComponent: () => import('./products/components/products.component').then(c => c.ProductsComponent)
  },
 {
  path: 'blogs',
  loadChildren: () => import('./blog/blog.module').then(m => m.BlogModule)
},

  {
    path: 'login',
    loadComponent: () => import('./auth/login/login.component').then(c => c.LoginComponent)
  },  
  {
    path: 'register',
    loadComponent: () => import('./auth/register/register.component').then(c => c.RegisterComponent)
  },
  {
    path: 'forgot-password',
    loadComponent: () => import('./auth/forgot-password/forgot-password').then(c => c.ForgotPasswordComponent)
  },
  {
    path: 'reset-password',
    loadComponent: () => import('./auth/forgot-password/reset-password/reset-password.component').then(c => c.ResetPasswordComponent)
  },
  {
    path: 'cart',
    loadComponent: () => import('./cart/components/cart-page/cart-page.component').then(c => c.CartPageComponent)
  },
  {
    path: 'cartSide',
    loadComponent: () => import('./cart/components/cart-sidebar/cart-sidebar.component').then(c => c.CartSidebarComponent)
  },
  {
    path: 'checkout',
    loadComponent: () => import('./checkout/checkout/checkout.component').then(c => c.CheckoutComponent),
    canActivate: [AuthGuard]
  },
  {
    path: 'confirmation-modal',
    loadComponent: () => import('./confirmation-modal/confirmation-modal/confirmation-modal.component').then(c => c.ConfirmationModalComponent)
  },
  {
    path: 'favorites',
    loadComponent: () => import('./favorites/components/favorite.component').then(c => c.FavoritesComponent)
  },

  { path: '**', redirectTo: 'home' },
];