import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { Allowance } from 'src/app/allowances/shared/allowance';
import { AllowanceService } from 'src/app/allowances/allowance.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-edit-allowance',
  templateUrl: './edit-allowance.component.html',
  styleUrls: ['./edit-allowance.component.scss'],
})
export class EditAllowanceComponent implements OnInit {
  allowance: Allowance;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  allowanceId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(
    private allowanceService: AllowanceService,
    private route: ActivatedRoute,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadAllowance();
  }

  onSave(allowance: Allowance): void {
    this.allowanceService.update(allowance, this.allowanceId).subscribe(
      () => {
        this.displaySuccess();
        this.router.navigate(['allowances', this.allowanceId]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to update allowance.')
    );
  }

  onCancel(): void {
    this.router.navigate(['allowances', this.allowanceId]);
  }

  private loadAllowance(): void {
    this.allowanceService.get(this.allowanceId).subscribe((data) => {
      this.allowance = data;

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
