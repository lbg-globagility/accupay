import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-confirmation-dialog',
  templateUrl: './confirmation-dialog.component.html',
  styleUrls: ['./confirmation-dialog.component.scss'],
})
export class ConfirmationDialogComponent {
  title: string;
  content: string;
  confirmButtonText: string;
  confirmButtonColor: string;

  constructor(
    private dialogRef: MatDialogRef<ConfirmationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    this.title = data.title;
    this.content = data.content;
    this.confirmButtonText = data.confirmButtonText;
    this.confirmButtonColor = data.confirmButtonColor ?? 'warn';
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
