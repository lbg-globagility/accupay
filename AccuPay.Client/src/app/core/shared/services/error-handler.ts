import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class ErrorHandler {
  constructor(private snackBar: MatSnackBar) {}
  /**
   *
   * @param httpError any The http error object.
   * @param defaultMessage string The default message if it is not a 400 error.
   */
  badRequest(httpError: any, defaultMessage: string): void {
    let message: string = defaultMessage;

    if (httpError && httpError.status == 400 && httpError?.error?.Error) {
      message = httpError.error.Error;
    }
    this.snackBar.open(message, null, {
      duration: 5000,
      panelClass: ['mat-toolbar', 'mat-warn'],
    });
  }
}
