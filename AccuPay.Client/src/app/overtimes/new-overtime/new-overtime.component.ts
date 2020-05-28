import Swal from 'sweetalert2';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Overtime } from 'src/app/overtimes/shared/overtime';
import { OvertimeService } from 'src/app/overtimes/overtime.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-new-overtime',
  templateUrl: './new-overtime.component.html',
  styleUrls: ['./new-overtime.component.scss'],
})
export class NewOvertimeComponent {
  constructor(
    private overtimeService: OvertimeService,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  onSave(overtime: Overtime): void {
    this.overtimeService.create(overtime).subscribe(
      (x) => {
        this.displaySuccess();
        this.router.navigate(['overtimes', x.id]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to create overtime.')
    );
  }

  onCancel(): void {
    this.router.navigate(['overtimes']);
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new overtime!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
