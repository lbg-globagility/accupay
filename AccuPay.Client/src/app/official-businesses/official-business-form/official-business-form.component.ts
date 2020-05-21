import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Employee } from 'src/app/employees/shared/employee';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { OfficialBusiness } from 'src/app/official-businesses/shared/official-business';
import { OfficialBusinessService } from 'src/app/official-businesses/official-business.service';
import { TimeParser } from 'src/app/core/shared/services/time-parser';
import * as moment from 'moment';
import { cloneDeep } from 'lodash';

@Component({
  selector: 'app-official-business-form',
  templateUrl: './official-business-form.component.html',
  styleUrls: ['./official-business-form.component.scss'],
})
export class OfficialBusinessFormComponent implements OnInit {
  @Input()
  officialBusiness: OfficialBusiness;

  @Output()
  save: EventEmitter<OfficialBusiness> = new EventEmitter();

  @Output()
  cancel: EventEmitter<any> = new EventEmitter();

  form: FormGroup = this.fb.group({
    id: [null],
    employeeId: [null, [Validators.required]],
    startDate: [null, [Validators.required]],
    startTime: [null, Validators.required],
    endTime: [null, Validators.required],
    status: [null, Validators.required],
    reason: [null],
    comments: [null],
  });

  employees: Employee[];
  statusList: string[];

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private officialBusinessService: OfficialBusinessService,
    private timeParser: TimeParser
  ) {}

  ngOnInit(): void {
    this.loadOfficialBusinessStatusList();

    if (this.officialBusiness != null) {
      this.form.get('employeeId').disable();
      this.form.patchValue(this.officialBusiness);
    } else {
      this.loadEmployees();
    }
    this.form.patchValue({
      startTime: this.timeParser.toInputTime(this.form.get('startTime').value),
      endTime: this.timeParser.toInputTime(this.form.get('endTime').value),
    });
  }

  private loadOfficialBusinessStatusList(): void {
    this.officialBusinessService.getStatusList().subscribe((data) => {
      this.statusList = data;
    });
  }

  private loadEmployees(): void {
    this.employeeService.getAll().subscribe((data) => {
      this.employees = data;
    });
  }

  onSave(): void {
    if (!this.form.valid) {
      return;
    }

    const officialBusiness = cloneDeep(this.form.value as OfficialBusiness);

    officialBusiness.startTime = this.timeParser.parse(
      moment(officialBusiness.startDate),
      officialBusiness.startTime
    );
    officialBusiness.endTime = this.timeParser.parse(
      moment(officialBusiness.startDate),
      officialBusiness.endTime
    );

    if (!officialBusiness.startTime || !officialBusiness.endTime) {
      officialBusiness.startTime = null;
      officialBusiness.endTime = null;
    }

    this.save.emit(officialBusiness);
  }

  onCancel(): void {
    this.cancel.emit();
  }
}
