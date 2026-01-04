import {Component, OnInit} from '@angular/core';
import {
  Favorite,
  FavoriteGetAllRequest,
  FavoriteGetAllResponse,
  FavoritesGetAllService
} from '../services/favorite-get-all-endpoint.service';
import {Form, FormBuilder, FormGroup} from '@angular/forms';
import {MyAuthService} from '../../core/services/auth/my-auth.service';
import {
  FavoriteToggleEndpointService
} from '../../products/services/product-endpoints/favorites-toggle-endpoint.service';
import {CartService} from '../../cart/services/cart.service';
import {MySnackbarHelperService} from '../../modules/shared/snackbars/my-snackbar-helper.service';

@Component({
  selector: 'app-favorites',
  templateUrl: 'favorite.component.html',
  styleUrls: ['favorite.component.css'],
  standalone: false
})

export class FavoritesComponent implements OnInit {
  sortBy: string = 'newest';
  filterForm!: FormGroup;
  favorites: Favorite[] = [];
  isAdmin = false;

  constructor(
    private favoriteGetAllService: FavoritesGetAllService,
    private fb: FormBuilder,
    public authService: MyAuthService,
    private favoriteToggleService: FavoriteToggleEndpointService,
    private cartService: CartService,
    private snackbar: MySnackbarHelperService,
  ) {
  }

  ngOnInit() {
    this.isAdmin = this.authService.isAdmin();

    this.filterForm = this.fb.group({
      sortBy: ['newest']
    })

    this.filterForm.get('sortBy')?.valueChanges.subscribe(value => {
      console.log("sort changed:", value);
      setTimeout(() => this.loadFavorite(), 0);
    });

    this.loadFavorite();
  }

  private loadFavorite() {
    const request: FavoriteGetAllRequest = {
      sortBy: this.filterForm.value.sortBy || 'newest',
      pageNumber: 1,
      pageSize: 20
    }

    this.favoriteGetAllService.handleAsync(request).subscribe({
      next: (response: FavoriteGetAllResponse) => {
        this.favorites = response.favorites;
      },
      error: (err: any) => console.log(err)
    });
  }

  toggleFavorite(id: number) {
    this.favoriteToggleService.handleAsync(id)
      .subscribe(result =>
        this.loadFavorite()
      );
  }

  addToCart(productId: number): void {
    this.cartService.addToCart(productId, 1).subscribe({
      error: (err) => {
        this.snackbar.showMessage('Unfortunately ,this product isnt awailable in the wanted quantity.')
      }
    });
  }
}
