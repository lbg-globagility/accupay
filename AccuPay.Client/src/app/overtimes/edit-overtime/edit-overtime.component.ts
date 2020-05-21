import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BehaviorSubject } from 'rxjs';
import { Overtime } from 'src/app/overtimes/shared/overtime';
import { OvertimeService } from 'src/app/overtimes/overtime.service';

@Component({
  selector: 'app-edit-overtime',
  templateUrl: './edit-overtime.component.html',
  styleUrls: ['./edit-overtime.component.scss'],
})
export class EditOvertimeComponent implements OnInit {
  overtime: Overtime;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  overtimeId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(
    private overtimeService: OvertimeService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadOvertime();
  }

  onSave(overtime: Overtime): void {
    this.overtimeService.update(overtime, this.overtimeId).subscribe(
      () => {
        this.displaySuccess();
        this.router.navigate(['overtimes', this.overtimeId]);
      },
      (err) => this.showErrorDialog(err)
    );
  }

  onCancel(): void {
    this.router.navigate(['overtimes', this.overtimeId]);
  }

  private loadOvertime(): void {
    this.overtimeService.get(this.overtimeId).subscribe((data) => {
      this.overtime = data;

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
    let message: string = 'Failed to update overtime.';

    if (err && err.status == 400) {
      message = err.error.Error;
    }
    this.snackBar.open(message, null, {
      duration: 2000,
      panelClass: ['mat-toolbar', 'mat-warn'],
    });
  }
}
