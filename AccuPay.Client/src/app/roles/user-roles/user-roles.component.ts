import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/users/user.service';
import { RoleService } from 'src/app/roles/services/role.service';
import { User } from 'src/app/users/shared/user';
import { Role } from 'src/app/roles/shared/role';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { LoadingState } from 'src/app/core/states/loading-state';
import { UserRole } from 'src/app/roles/shared/user-role';

@Component({
  selector: 'app-user-roles',
  templateUrl: './user-roles.component.html',
  styleUrls: ['./user-roles.component.scss'],
})
export class UserRolesComponent implements OnInit {
  loadingState: LoadingState = new LoadingState();

  form: FormGroup = this.fb.group({
    users: this.fb.array([]),
  });

  displayedColumns: string[] = ['user', 'none'];

  users: User[] = [];

  roles: Role[] = [];

  get userControls(): FormArray {
    return this.form.get('users') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private roleService: RoleService
  ) {}

  ngOnInit(): void {
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
    const value = this.form.value;

    this.roleService.updateUserRoles(value.users).subscribe();
    console.log(value);
  }

  private setupRoles(roles: Role[]) {
    this.roles = roles;
    const roleNames = roles.map((r) => r.name);
    this.displayedColumns.push(...roleNames);
  }

  private setupUsers(users: User[], userRoles: UserRole[]) {
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
}
