import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BehaviorSubject } from 'rxjs';
import { Leave } from 'src/app/leaves/shared/leave';
import { LeaveService } from 'src/app/leaves/leave.service';

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
    private snackBar: MatSnackBar
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
      (err) => this.showErrorDialog(err)
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

  private showErrorDialog(err): void {
    console.log(err);

    let message: string = 'Failed to create the leave';

    if (err && err.status == 400) {
      message = err.error.Error;
    }
    this.snackBar.open(message, null, {
      duration: 2000,
      panelClass: ['mat-toolbar', 'mat-warn'],
    });
  }
}
