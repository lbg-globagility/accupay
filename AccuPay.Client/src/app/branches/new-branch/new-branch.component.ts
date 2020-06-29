import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BranchService } from 'src/app/branches/services/branch.service';
import { Branch } from 'src/app/branches/shared/branch';
import Swal from 'sweetalert2';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-new-branch',
  templateUrl: './new-branch.component.html',
  styleUrls: ['./new-branch.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class NewBranchComponent implements OnInit {
  constructor(
    private branchService: BranchService,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {}

  onSave(branch: Branch) {
    this.branchService.create(branch).subscribe({
      next: (result) => {
        this.router.navigate(['branches']);
        this.displaySuccess();
      },
      error: (err) =>
        this.errorHandler.badRequest(err, 'Failed to create new branch'),
    });
  }

  onCancel() {
    this.router.navigate(['branches']);
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new branch!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
