import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ViewEmployeeComponent } from './view-employee/view-employee.component';
import { EditEmployeeComponent } from './edit-employee/edit-employee.component';
import { NewEmployeeComponent } from './new-employee/new-employee.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { SharedModule } from '../shared/shared.module';
import { EmployeeFormComponent } from './employee-form/employee-form.component';
import { EmployeesComponent } from './employees/employees.component';

@NgModule({
  declarations: [
    ViewEmployeeComponent,
    EditEmployeeComponent,
    NewEmployeeComponent,
    EmployeeListComponent,
    EmployeeFormComponent,
    EmployeesComponent,
  ],
  imports: [SharedModule],
})
export class EmployeesModule {}
