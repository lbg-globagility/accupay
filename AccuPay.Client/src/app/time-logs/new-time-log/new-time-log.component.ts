import { Component, OnInit } from '@angular/core';
import { TimeLogService } from '../time-log.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { Router } from '@angular/router';
import { TimeLog } from '../shared/time-log';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-new-time-log',
  templateUrl: './new-time-log.component.html',
  styleUrls: ['./new-time-log.component.scss']
})
export class NewTimeLogComponent  {
  constructor(
    private timeLogService: TimeLogService,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  onSave(timeLog: TimeLog): void {
    this.timeLogService.create(timeLog).subscribe(
      (x) => {
        this.displaySuccess();
        this.router.navigate(['time-logs', x.id]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to create time-log.')
    );
  }

  onCancel(): void {
    this.router.navigate(['time-logs']);
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new time-log!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
