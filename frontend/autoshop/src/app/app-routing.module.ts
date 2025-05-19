import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
//import {BlogPostsComponent} from './modules/administration/blog-posts/blog-posts.component';
//ii//import {AdministrationComponent} from './modules/administration/administration.component';
import {HomePageComponent} from './modules/administration/home-page/home-page.component';
//import {BlogListComponent} from './modules/blogs/blog-posts.component';
import {BlogDetailsComponent} from './modules/blogs/blog/blog-post.component';
import {RegisterComponent} from './register/register.component';
// Importujte komponentu
import { LoginComponent } from './login/login.component';
import { ForgotPasswordComponent } from './login/forgot-password';
const routes: Routes = [
 // { //path: 'administration', component: AdministrationComponent, children: [
      //{ path: '', redirectTo: 'admin/home-page', pathMatch: 'full' },
      //{ path: 'admin/blog-posts', component: BlogPostsComponent },
     // { path: 'admin/home-page', component: HomePageComponent },
   // ],
  //},
 // { path: 'blogs', component: BlogListComponent },  // Definišite rutu za komponentu
  //{ path: 'blog/:id', component: BlogDetailsComponent},
  {path:'login', component: LoginComponent},
  {path:'register', component: RegisterComponent},
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component:HomePageComponent },
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: '**', redirectTo: 'home' }, // Dodajte komponentu za početnu stranicu
  //{ path: '', redirectTo: '/admin/blog-posts', pathMatch: 'full' }, // Možete postaviti početnu stranicu
{path: 'forgot-password',component: ForgotPasswordComponent}


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],  // Dodajte rute u aplikaciju
  exports: [RouterModule]
})
export class AppRoutingModule { }
