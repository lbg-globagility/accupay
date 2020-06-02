import { Component, OnInit } from '@angular/core';
import { TimeLog } from '../shared/time-log';
import { BehaviorSubject } from 'rxjs';
import { TimeLogService } from '../time-log.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-edit-time-log',
  templateUrl: './edit-time-log.component.html',
  styleUrls: ['./edit-time-log.component.scss']
})
export class EditTimeLogComponent implements OnInit {
  timeLog: TimeLog;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  timeLogId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(
    private timeLogService: TimeLogService,
    private route: ActivatedRoute,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadShift();
  }

  onSave(timeLog: TimeLog): void {
    this.timeLogService.update(timeLog, this.timeLogId).subscribe(
      () => {
        this.displaySuccess();
        this.router.navigate(['time-logs', this.timeLogId]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to update time-log.')
    );
  }

  onCancel(): void {
    this.router.navigate(['time-logs', this.timeLogId]);
  }

  private loadShift(): void {
    this.timeLogService.get(this.timeLogId).subscribe((data) => {
      this.timeLog = data;

      this.isLoading.next(true);
    });
  }

  private displaySuccess(): void {
    Swal.fire({
      title: 'Success',
      text: 'Successfully updated!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
