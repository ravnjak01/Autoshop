import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { appRoutes as routes } from './app.routes';

// Importujte komponentu


@NgModule({
  imports: [RouterModule.forRoot(routes2,routes, { enableTracing: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
