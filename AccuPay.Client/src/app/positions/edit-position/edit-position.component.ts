import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { Position } from 'src/app/positions/shared/position';
import { PositionService } from 'src/app/positions/position.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-edit-position',
  templateUrl: './edit-position.component.html',
  styleUrls: ['./edit-position.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class EditPositionComponent implements OnInit {
  position: Position;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  positionId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(
    private positionService: PositionService,
    private route: ActivatedRoute,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadPosition();
  }

  onSave(position: Position): void {
    this.positionService.update(position, this.positionId).subscribe(
      () => {
        this.displaySuccess();
        this.router.navigate(['positions', this.positionId]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to update position.')
    );
  }

  onCancel(): void {
    this.router.navigate(['positions', this.positionId]);
  }

  private loadPosition(): void {
    this.positionService.get(this.positionId).subscribe((data) => {
      this.position = data;

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
