import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { Overtime } from 'src/app/overtimes/shared/overtime';
import { OvertimeService } from 'src/app/overtimes/overtime.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

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
    private errorHandler: ErrorHandler
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
      (err) => this.errorHandler.badRequest(err, 'Failed to update overtime.')
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
}
