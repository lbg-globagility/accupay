import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { OfficialBusiness } from 'src/app/official-businesses/shared/official-business';
import { LoadingState } from 'src/app/core/states/loading-state';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { TimeParser } from 'src/app/core/shared/services/time-parser';
import { cloneDeep } from 'lodash';
import * as moment from 'moment';

@Component({
  selector: 'app-selfserve-official-business-form',
  templateUrl: './selfserve-official-business-form.component.html',
  styleUrls: ['./selfserve-official-business-form.component.scss'],
})
export class SelfserveOfficialBusinessFormComponent implements OnInit {
  @Input()
  officialBusiness: OfficialBusiness;

  @Output()
  save: EventEmitter<OfficialBusiness> = new EventEmitter();

  @Output()
  cancel: EventEmitter<OfficialBusiness> = new EventEmitter();

  savingState: LoadingState = new LoadingState();

  form: FormGroup = this.fb.group({
    startTime: [null],
    endTime: [null],
    startDate: [null, [Validators.required]],
    endDate: [null],
    status: ['Pending', [Validators.required]],
    reason: [null],
    comments: [null],
  });

  constructor(private fb: FormBuilder, private timeParser: TimeParser) {}

  ngOnInit(): void {}

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
}
