import { Component, OnInit } from '@angular/core';
import { Shift } from '../shared/shift';
import { BehaviorSubject } from 'rxjs';
import { ShiftService } from '../shift.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-view-shift',
  templateUrl: './view-shift.component.html',
  styleUrls: ['./view-shift.component.scss']
})
export class ViewShiftComponent implements OnInit {
  shift: Shift;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  shiftId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(
    private shiftService: ShiftService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadshift();
  }

  confirmDelete(): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete shift',
        content: 'Are you sure you want to delete this shift?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result !== true) return;

      this.shiftService.delete(this.shiftId).subscribe(() => {
        this.router.navigate(['shifts']);
        Swal.fire({
          title: 'Deleted',
          text: `The shift was successfully deleted.`,
          icon: 'success',
          showConfirmButton: true,
        });
      });
    });
  }

  private loadshift(): void {
    this.shiftService.get(this.shiftId).subscribe((data) => {
      this.shift = data;

      this.isLoading.next(true);
    });
  }
}