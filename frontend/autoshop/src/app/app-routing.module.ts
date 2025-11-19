import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {BlogPostsComponent} from './modules/administration/blog-posts/blog-posts.component';
import {AdministrationComponent} from './modules/administration/administration.component';
import {HomePageComponent} from './modules/administration/home-page/home-page.component';
import {BlogListComponent} from './modules/blogs/blog-posts.component';
import {BlogDetailsComponent} from './modules/blogs/blog/blog-post.component';
import {RegisterComponent} from './register/register.component';
import {ProductListComponent} from './modules/products/products.component';
import {DiscountsComponent} from './modules/administration/discount/discount.component';
// Importujte komponentu
const routes2: Routes = [
  { path: 'administration', component: AdministrationComponent, children: [
      { path: '', redirectTo: 'admin/home-page', pathMatch: 'full' },
      { path: 'admin/blog-posts', component: BlogPostsComponent },
      { path: 'admin/home-page', component: HomePageComponent },
    ],
  },
  { path: 'blogs', component: BlogListComponent },  // Definišite rutu za komponentu
  { path: 'blog/:id', component: BlogDetailsComponent},
  {path: 'products', component: ProductListComponent},
  {path:'register', component: RegisterComponent},
  //{ path: '', redirectTo: '/admin/blog-posts', pathMatch: 'full' }, // Možete postaviti početnu stranicu
];
import { appRoutes as routes } from './app.routes';
@NgModule({
  imports: [RouterModule.forRoot(routes2,routes, { enableTracing: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
