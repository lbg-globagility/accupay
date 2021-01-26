import { NgModule } from '@angular/core';
import { ViewEmployeeComponent } from './view-employee/view-employee.component';
import { EditEmployeeComponent } from './edit-employee/edit-employee.component';
import { NewEmployeeComponent } from './new-employee/new-employee.component';
import { SharedModule } from '../shared/shared.module';
import { EmployeeFormComponent } from './employee-form/employee-form.component';
import { EmployeesComponent } from './employees/employees.component';

@NgModule({
  declarations: [
    ViewEmployeeComponent,
    EditEmployeeComponent,
    NewEmployeeComponent,
    EmployeeFormComponent,
    EmployeesComponent,
  ],
  imports: [SharedModule],
})
export class EmployeesModule {}
