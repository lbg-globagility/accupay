import { Component} from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-delete-salary-confirmation',
  templateUrl: './delete-salary-confirmation.component.html',
  styleUrls: ['./delete-salary-confirmation.component.scss']
})
export class DeleteSalaryConfirmationComponent {

  constructor(private dialogRef: MatDialogRef<DeleteSalaryConfirmationComponent>) { }

  onCancel(): void {
    this.dialogRef.close();
  }

}
