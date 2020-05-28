import Swal from 'sweetalert2';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { OfficialBusiness } from 'src/app/official-businesses/shared/official-business';
import { OfficialBusinessService } from 'src/app/official-businesses/official-business.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-new-official-business',
  templateUrl: './new-official-business.component.html',
  styleUrls: ['./new-official-business.component.scss'],
})
export class NewOfficialBusinessComponent {
  constructor(
    private officialBusinessService: OfficialBusinessService,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  onSave(officialBusiness: OfficialBusiness): void {
    this.officialBusinessService.create(officialBusiness).subscribe(
      (x) => {
        this.displaySuccess();
        this.router.navigate(['official-businesses', x.id]);
      },
      (err) =>
        this.errorHandler.badRequest(err, 'Failed to create official business.')
    );
  }

  onCancel(): void {
    this.router.navigate(['official-businesses']);
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new official business!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
