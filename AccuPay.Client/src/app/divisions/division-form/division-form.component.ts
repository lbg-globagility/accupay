import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Division } from '../shared/division';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { DivisionService } from '../division.service';
import { cloneDeep } from 'lodash';

@Component({
  selector: 'app-division-form',
  templateUrl: './division-form.component.html',
  styleUrls: ['./division-form.component.scss']
})
export class DivisionFormComponent implements OnInit {
  @Input()
  division: Division;

  @Output()
  save: EventEmitter<Division> = new EventEmitter();

  @Output()
  cancel: EventEmitter<any> = new EventEmitter();

  form: FormGroup = this.fb.group({
    id: [null],
    name: [null, [Validators.required]],
    parentId: [null, [Validators.required]],
    divisionType: [null],
    workDaysPerYear: [null, [Validators.required]],
    automaticOvertimeFiling: [null],
    philHealthDeductionSchedule: [null, [Validators.required]],
    sssDeductionSchedule: [null, [Validators.required]],
    pagIBIGDeductionSchedule: [null, [Validators.required]],
    withholdingTaxSchedule: [null, [Validators.required]],    
  });

  parentDivisions: Division[];
  divisionTypes: string[];
  deductionSchedules : string[];

  constructor(
    private fb: FormBuilder,
    private divisionService: DivisionService,
  ) {}

  ngOnInit(): void {
    
    this.loadParentDivisions();
    this.loadDivisionTypes();
    this.loadDeductionSchedules();

    if (this.division != null) {
      this.form.patchValue(this.division);
    }

  }
  private loadDeductionSchedules() {
    this.divisionService.getDeductionSchedules().subscribe((data) => {
      this.deductionSchedules = data;
    });
  }

  private loadDivisionTypes() {
    this.divisionService.getDivisionTypes().subscribe((data) => {
      this.divisionTypes = data;
    });
  }

  private loadParentDivisions(): void {
    this.divisionService.getAllParents().subscribe((data) => {
      this.parentDivisions = data;
    });
  }

  onSave(): void {
    if (!this.form.valid) {
      return;
    }

    const division = cloneDeep(this.form.value as Division);

    console.log(division);

    this.save.emit(division);
  }

  onCancel(): void {
    this.cancel.emit();
  }
}
