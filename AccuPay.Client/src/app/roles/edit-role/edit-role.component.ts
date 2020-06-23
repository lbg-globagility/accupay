import { Component, OnInit, ViewChild } from '@angular/core';
import { RoleService } from 'src/app/roles/services/role.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Role } from 'src/app/roles/shared/role';
import { RoleFormComponent } from 'src/app/roles/role-form/role-form.component';
import Swal from 'sweetalert2';
import { LoadingState } from 'src/app/core/states/loading-state';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-edit-role',
  templateUrl: './edit-role.component.html',
  styleUrls: ['./edit-role.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class EditRoleComponent implements OnInit {
  @ViewChild(RoleFormComponent)
  roleForm: RoleFormComponent;

  roleId: string = this.route.snapshot.paramMap.get('id');

  role: Role;

  savingState: LoadingState = new LoadingState();

  constructor(
    private roleService: RoleService,
    private route: ActivatedRoute,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadRole();
  }

  loadRole() {
    this.roleService
      .getById(this.roleId)
      .subscribe((role) => (this.role = role));
  }

  save() {
    if (!this.roleForm.valid) {
      return;
    }

    this.savingState.changeToLoading();

    const role = this.roleForm.value;
    this.roleService.update(this.roleId, role).subscribe({
      next: () => {
        this.savingState.changeToSuccess();
        this.displaySuccess();
        this.router.navigate(['roles']);
      },
      error: (err) => {
        this.savingState.changeToError();
        this.errorHandler.badRequest(err, 'Failed to save role.');
      },
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
