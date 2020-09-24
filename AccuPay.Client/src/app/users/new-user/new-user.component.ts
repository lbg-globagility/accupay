import { Component, OnInit, ViewChild } from '@angular/core';
import { UserService } from 'src/app/users/user.service';
import { User } from 'src/app/users/shared/user';
import { Router } from '@angular/router';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import { MatDialog } from '@angular/material/dialog';
import { UnregisteredEmployeeListComponent } from '../unregistered-employee-list/unregistered-employee-list.component';
import { cloneDeep } from 'lodash';
import { Employee } from 'src/app/employees/shared/employee';
import { PermissionTypes } from 'src/app/core/auth';
import { UserFormComponent } from '../user-form/user-form.component';
import { EmployeeUserModel } from '../shared/user-form-user';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-new-user',
  templateUrl: './new-user.component.html',
  styleUrls: ['./new-user.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class NewUserComponent implements OnInit {
  @ViewChild(UserFormComponent)
  userForm: UserFormComponent;

  readonly PermissionTypes = PermissionTypes;

  constructor(
    private userService: UserService,
    private router: Router,
    private errorHandler: ErrorHandler,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {}

  save(user: User) {
    this.userService.create(user, false).subscribe(
      (u) => {
        this.displaySuccess();
        this.router.navigate(['users', u.id]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to create user.')
    );
  }

  cancel() {
    this.router.navigate(['security', 'users']);
  }

  openUnregisteredEmployeesDialog() {
    this.dialog
      .open(UnregisteredEmployeeListComponent)
      .afterClosed()
      .subscribe((employees) => {
        if (employees == null) {
          return;
        }

        const selectedEmployees = cloneDeep(employees as Employee[]);

        if (selectedEmployees.length == 1) {
          const employee = selectedEmployees[0] as Employee;

          const newUser: EmployeeUserModel = {
            firstName: employee.firstName,
            lastName: employee.lastName,
            email: employee.emailAddress,
            employeeId: employee.id,
          };

          this.userForm.patchUser(newUser);
        } else {
          console.log(selectedEmployees);
        }
      });
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new user!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
