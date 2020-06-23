import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/users/user.service';
import { RoleService } from 'src/app/roles/services/role.service';
import { User } from 'src/app/users/shared/user';
import { Role } from 'src/app/roles/shared/role';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { LoadingState } from 'src/app/core/states/loading-state';
import { UserRole } from 'src/app/roles/shared/user-role';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-user-roles',
  templateUrl: './user-roles.component.html',
  styleUrls: ['./user-roles.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class UserRolesComponent implements OnInit {
  readonly defaultColumns: string[] = ['user', 'none'];

  displayedColumns: string[] = [];

  loadingState: LoadingState = new LoadingState();

  savingState: LoadingState = new LoadingState();

  form: FormGroup = this.fb.group({
    users: this.fb.array([]),
  });

  users: User[] = [];

  roles: Role[] = [];

  get userControls(): FormArray {
    return this.form.get('users') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private roleService: RoleService,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.loadingState.changeToLoading();

    const $users = this.userService.getAll();
    const $roles = this.roleService.getAll();
    const $userRoles = this.roleService.getUserRoles();

    forkJoin([$users, $roles, $userRoles]).subscribe(
      ([users, roles, userRoles]) => {
        this.setupRoles(roles);
        this.setupUsers(users, userRoles);

        this.loadingState.changeToSuccess();
      }
    );
  }

  save() {
    this.savingState.changeToLoading();

    const value = this.form.value;

    this.roleService.updateUserRoles(value.users).subscribe({
      next: () => {
        this.displaySuccess();
        this.savingState.changeToSuccess();
      },
      error: (err) => {
        this.savingState.changeToError();
        this.errorHandler.badRequest(err, 'Failed to update user roles');
      },
    });
  }

  private setupRoles(roles: Role[]) {
    this.roles = roles;
    const roleNames = roles.map((r) => r.name);
    this.displayedColumns = this.defaultColumns.concat(...roleNames);
  }

  private setupUsers(users: User[], userRoles: UserRole[]) {
    this.userControls.clear();

    this.users = users;

    const matchedUsers = users.map((user) => ({
      user,
      userRole: userRoles.find((u) => u.userId === user.id),
    }));

    matchedUsers.forEach((u) => this.addUserControl(u.user, u.userRole));
  }

  private addUserControl(user: User, userRole: UserRole) {
    this.userControls.push(
      this.fb.group({
        userId: [user.id],
        roleId: [userRole?.roleId],
      })
    );
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully updated user roles!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }

  cancel() {
    this.loadData();
  }
}
