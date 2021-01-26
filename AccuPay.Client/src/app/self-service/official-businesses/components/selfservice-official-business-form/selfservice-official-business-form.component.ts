import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as moment from 'moment';
import { cloneDeep } from 'lodash';
import { TimeParser } from 'src/app/core/shared/services/time-parser';
import { LoadingState } from 'src/app/core/states/loading-state';
import { OfficialBusiness } from 'src/app/official-businesses/shared/official-business';

@Component({
  selector: 'app-selfservice-official-business-form',
  templateUrl: './selfservice-official-business-form.component.html',
  styleUrls: ['./selfservice-official-business-form.component.scss'],
})
export class SelfserviceOfficialBusinessFormComponent implements OnInit {
  @Input()
  officialBusiness: OfficialBusiness;

  savingState: LoadingState = new LoadingState();

  form: FormGroup = this.fb.group({
    startDate: [null, Validators.required],
    startTime: [null],
    endTime: [null],
    endDate: [null],
    reason: [null],
    isWholeDay: [false],
  });

  constructor(private fb: FormBuilder, private timeParser: TimeParser) {}

  ngOnInit(): void {
    this.form.get('isWholeDay').valueChanges.subscribe((checked: boolean) => {
      this.disableTimeInputs(checked);
    });
  }

  get valid(): boolean {
    this.form.markAllAsTouched();
    return this.form.valid;
  }

  get value(): OfficialBusiness {
    const officialBusiness = cloneDeep(this.form.value as OfficialBusiness);

    officialBusiness.startTime = this.timeParser.parse(
      moment(officialBusiness.startDate),
      officialBusiness.startTime
    );
    officialBusiness.endTime = this.timeParser.parse(
      moment(officialBusiness.startDate),
      officialBusiness.endTime
    );

    return officialBusiness;
  }

  private disableTimeInputs(checked: boolean): void {
    if (checked) {
      this.form.get('startTime').disable();
      this.form.get('endTime').disable();
    } else {
      this.form.get('startTime').enable();
      this.form.get('endTime').enable();
    }
  }
}
