import { Component, OnInit } from '@angular/core';
import { BranchService } from 'src/app/branches/services/branch.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Branch } from 'src/app/branches/shared/branch';
import { LoadingState } from 'src/app/core/states/loading-state';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-edit-branch',
  templateUrl: './edit-branch.component.html',
  styleUrls: ['./edit-branch.component.scss'],
})
export class EditBranchComponent implements OnInit {
  state: LoadingState = new LoadingState();

  branchId: number = +this.route.snapshot.paramMap.get('id');

  branch: Branch;

  constructor(
    private branchService: BranchService,
    private route: ActivatedRoute,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadBranch();
  }

  onSave(branch: Branch) {
    this.branchService.update(this.branchId, branch).subscribe({
      next: () => {
        this.router.navigate(['branches']);
        this.displaySuccess();
      },
      error: (err) => {
        this.errorHandler.badRequest(err, 'Failed to update branch');
      },
    });
  }

  onCancel() {
    this.router.navigate(['branches']);
  }

  private loadBranch() {
    this.branchService.getById(this.branchId).subscribe((branch) => {
      this.branch = branch;
      this.state.changeToSuccess();
    });
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully updated a new branch!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
