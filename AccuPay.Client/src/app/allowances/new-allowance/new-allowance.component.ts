import Swal from 'sweetalert2';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Allowance } from 'src/app/allowances/shared/allowance';
import { AllowanceService } from 'src/app/allowances/allowance.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-new-allowance',
  templateUrl: './new-allowance.component.html',
  styleUrls: ['./new-allowance.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class NewAllowanceComponent {
  constructor(
    private allowanceService: AllowanceService,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  onSave(allowance: Allowance): void {
    this.allowanceService.create(allowance).subscribe(
      (result) => {
        this.displaySuccess();
        this.router.navigate(['allowances', result.id]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to create allowance.')
    );
  }

  onCancel(): void {
    this.router.navigate(['allowances']);
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new allowance!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
