import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { Leave } from 'src/app/leaves/shared/leave';
import { LeaveService } from 'src/app/leaves/leave.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-edit-leave',
  templateUrl: './edit-leave.component.html',
  styleUrls: ['./edit-leave.component.scss'],
})
export class EditLeaveComponent implements OnInit {
  leave: Leave;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  leaveId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(
    private leaveService: LeaveService,
    private route: ActivatedRoute,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadLeave();
  }

  onSave(leave: Leave): void {
    this.leaveService.update(leave, this.leaveId).subscribe(
      () => {
        this.displaySuccess();
        this.router.navigate(['leaves', this.leaveId]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to update leave.')
    );
  }

  onCancel(): void {
    this.router.navigate(['leaves', this.leaveId]);
  }

  private loadLeave(): void {
    this.leaveService.get(this.leaveId).subscribe((data) => {
      this.leave = data;

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
