import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {BlogPostsComponent} from './modules/administration/blog-posts/blog-posts.component';// Importujte komponentu

const routes: Routes = [
  { path: 'admin/blog-posts', component: BlogPostsComponent },  // Definišite rutu za komponentu
  { path: '', redirectTo: '/admin/blog-posts', pathMatch: 'full' }, // Možete postaviti početnu stranicu
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],  // Dodajte rute u aplikaciju
  exports: [RouterModule]
})
export class AppRoutingModule { }
