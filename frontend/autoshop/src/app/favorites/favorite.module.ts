import {NgModule} from '@angular/core';
import {FavoritesComponent} from './components/favorite.component';
import {NgForOf, NgIf} from '@angular/common';
import {ReactiveFormsModule} from '@angular/forms';
import {SharedModule} from '../modules/shared/shared.module';

@NgModule({
  declarations: [
    FavoritesComponent
  ],
  imports: [
    NgForOf,
    NgIf,
    ReactiveFormsModule,
    SharedModule
  ],
  exports: [
    FavoritesComponent
  ]
})
export class FavoriteModule {}
