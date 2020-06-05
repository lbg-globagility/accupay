import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Employee } from '../shared/employee';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-employee-form',
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.scss'],
})
export class EmployeeFormComponent implements OnInit {
  @Input()
  employee: Employee;

  @Input()
  uncontrollable = false;

  @Output()
  save: EventEmitter<Employee> = new EventEmitter();

  @Output()
  cancel = new EventEmitter<void>();

  form: FormGroup = this.fb.group({
    employeeNo: [null, Validators.required],
    firstName: [null, Validators.required],
    lastName: [null, Validators.required],
    middleName: [],
    birthdate: [, Validators.required],
    address: [],
    landlineNo: [],
    mobileNo: [],
    emailAddress: [],
    tin: [],
    sssNo: [],
    philHealthNo: [],
    pagIbigNo: [],
  });

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    if (this.employee != null) {
      this.form.patchValue(this.employee);
    }
  }

  onSave() {
    if (this.form.valid) {
      this.save.emit(this.form.value);
    }
  }

  getValue(property: string) {
    return this.form.get(property).value;
  }
}
