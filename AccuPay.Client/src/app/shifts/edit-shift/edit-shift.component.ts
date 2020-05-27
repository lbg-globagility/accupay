import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BehaviorSubject } from 'rxjs';
import { Shift } from 'src/app/shifts/shared/shift';
import { ShiftService } from 'src/app/shifts/shift.service';

@Component({
  selector: 'app-edit-shift',
  templateUrl: './edit-shift.component.html',
  styleUrls: ['./edit-shift.component.scss'],
})
export class EditShiftComponent implements OnInit {
  shift: Shift;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  shiftId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(
    private shiftService: ShiftService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadShift();
  }

  onSave(shift: Shift): void {
    this.shiftService.update(shift, this.shiftId).subscribe(
      () => {
        this.displaySuccess();
        this.router.navigate(['shifts', this.shiftId]);
      },
      (err) => this.showErrorDialog(err)
    );
  }

  onCancel(): void {
    this.router.navigate(['shifts', this.shiftId]);
  }

  private loadShift(): void {
    this.shiftService.get(this.shiftId).subscribe((data) => {
      this.shift = data;

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
    let message: string = 'Failed to update shift.';

    if (err && err.status == 400) {
      message = err.error.Error;
    }
    this.snackBar.open(message, null, {
      duration: 2000,
      panelClass: ['mat-toolbar', 'mat-warn'],
    });
  }
}
