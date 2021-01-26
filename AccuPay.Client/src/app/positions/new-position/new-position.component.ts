import Swal from 'sweetalert2';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Position } from 'src/app/positions/shared/position';
import { PositionService } from 'src/app/positions/position.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-new-position',
  templateUrl: './new-position.component.html',
  styleUrls: ['./new-position.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class NewPositionComponent {
  constructor(
    private positionService: PositionService,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  onSave(position: Position): void {
    this.positionService.create(position).subscribe(
      (x) => {
        this.displaySuccess();
        this.router.navigate(['positions', x.id]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to create position.')
    );
  }

  onCancel(): void {
    this.router.navigate(['positions']);
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new position!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
