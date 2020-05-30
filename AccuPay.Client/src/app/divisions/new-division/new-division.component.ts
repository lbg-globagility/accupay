import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { DivisionService } from '../division.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { Division } from '../shared/division';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-new-division',
  templateUrl: './new-division.component.html',
  styleUrls: ['./new-division.component.scss'],
})
export class NewDivisionComponent {
  constructor(
    private divisionService: DivisionService,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  onSave(division: Division): void {
    this.divisionService.create(division).subscribe(
      (x) => {
        this.displaySuccess();
        this.router.navigate(['divisions', x.id]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to create division.')
    );
  }

  onCancel(): void {
    this.router.navigate(['divisions']);
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new division!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
