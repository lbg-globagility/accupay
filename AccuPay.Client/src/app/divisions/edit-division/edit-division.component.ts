import { Component, OnInit } from '@angular/core';
import { Division } from '../shared/division';
import { BehaviorSubject } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { DivisionService } from '../division.service';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-edit-division',
  templateUrl: './edit-division.component.html',
  styleUrls: ['./edit-division.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class EditDivisionComponent implements OnInit {
  division: Division;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  divisionId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(
    private divisionService: DivisionService,
    private route: ActivatedRoute,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadDivision();
  }

  onSave(division: Division): void {
    this.divisionService.update(division, this.divisionId).subscribe(
      () => {
        this.displaySuccess();
        this.router.navigate(['positions', 'divisions', this.divisionId]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to update division.')
    );
  }

  onCancel(): void {
    this.router.navigate(['positions', 'divisions', this.divisionId]);
  }

  private loadDivision(): void {
    this.divisionService.get(this.divisionId).subscribe((data) => {
      this.division = data;

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
