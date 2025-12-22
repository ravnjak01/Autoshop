import {Injectable} from '@angular/core';
import {MatSnackBar, MatSnackBarRef, TextOnlySnackBar} from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class MySnackbarHelperService {

  constructor(private snackBar: MatSnackBar) {}

  /**
   * @param message Tekst poruke
   * @param type 'success' | 'error' | 'info'
   * @param duration Trajanje
   */
  showMessage(message: string, type: 'success' | 'error' | 'info' = 'info', duration: number = 4000): MatSnackBarRef<TextOnlySnackBar> {
    return this.snackBar.open(message, undefined, {
      duration: duration,
      horizontalPosition: 'center', // Obično ljepše izgleda desno
      verticalPosition: 'bottom',      // I gore (manje smeta contentu)
      panelClass: [`snackbar-${type}`] // Ovdje dodajemo klasu za CSS
    });
  }
}