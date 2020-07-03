import { Component, OnInit, ViewChild } from '@angular/core';
import { RoleService } from 'src/app/roles/services/role.service';
import { RoleFormComponent } from 'src/app/roles/role-form/role-form.component';
import { LoadingState } from 'src/app/core/states/loading-state';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-role',
  templateUrl: './new-role.component.html',
  styleUrls: ['./new-role.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class NewRoleComponent implements OnInit {
  savingState: LoadingState = new LoadingState();

  @ViewChild(RoleFormComponent)
  roleForm: RoleFormComponent;

  constructor(
    private roleService: RoleService,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {}

  save() {
    if (!this.roleForm.valid) {
      return;
    }

    this.savingState.changeToLoading();

    const role = this.roleForm.value;

    this.roleService.create(role).subscribe({
      next: () => {
        this.displaySuccess();
        this.router.navigate(['roles']);
      },
      error: (err) =>
        this.errorHandler.badRequest(err, 'Failed to create role'),
    });
  }

  cancel() {
    this.router.navigate(['roles']);
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new role!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
