import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormControl,
} from '@angular/forms';
import { Employee } from 'src/app/employees/shared/employee';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { Loan } from 'src/app/loans/shared/loan';
import { LoanService } from 'src/app/loans/loan.service';
import { SelectItem } from 'src/app/core/shared/select-item';
import { toNumber, round } from 'lodash';

@Component({
  selector: 'app-loan-form',
  templateUrl: './loan-form.component.html',
  styleUrls: ['./loan-form.component.scss'],
})
export class LoanFormComponent implements OnInit {
  @Input()
  loan: Loan;

  @Output()
  save: EventEmitter<Loan> = new EventEmitter();

  @Output()
  cancel: EventEmitter<any> = new EventEmitter();

  form: FormGroup = this.fb.group({
    id: [null],
    employeeId: [null, [Validators.required]],
    loanTypeId: [null, Validators.required],
    loanNumber: [null],
    startDate: [null, [Validators.required]],
    totalLoanAmount: [null, Validators.required],
    deductionAmount: [null, Validators.required],
    deductionPercentage: new FormControl(0, {
      validators: Validators.required,
      updateOn: 'blur',
    }),
    status: [null, Validators.required],
    deductionSchedule: [null, Validators.required],
    comments: [null],
  });

  employees: Employee[];
  loanTypes: SelectItem[];
  statusList: string[];
  deductionSchedules: string[];

  isFormInitialized: boolean = false;

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private loanService: LoanService
  ) {
    this.form
      .get('deductionPercentage')
      .valueChanges.subscribe(this.recomputeTotalLoanAmount());
  }

  ngOnInit(): void {
    this.loadLoanTypes();
    this.loadLoanStatusList();
    this.loadDeductionSchedules();

    if (this.loan != null) {
      this.form.get('employeeId').disable();
      this.form.patchValue(this.loan);
    } else {
      this.loadEmployees();
    }

    this.isFormInitialized = true;
  }

  private loadLoanTypes(): void {
    this.loanService.getLoanTypes().subscribe((data) => {
      this.loanTypes = data;
    });
  }

  private loadLoanStatusList(): void {
    this.loanService.getStatusList().subscribe((data) => {
      this.statusList = data;
    });
  }

  private loadDeductionSchedules(): void {
    this.loanService.getDeductionSchedules().subscribe((data) => {
      this.deductionSchedules = data;
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

    const loan = this.form.value as Loan;

    loan.totalLoanAmount = Number(loan.totalLoanAmount);

    this.save.emit(loan);
  }

  onCancel(): void {
    this.cancel.emit();
  }

  private recomputeTotalLoanAmount(): (value: any) => void {
    return () => {
      if (!this.isFormInitialized) return;

      const totalAmount = toNumber(this.form.get('totalLoanAmount').value);
      const deductionPercentage = toNumber(
        this.form.get('deductionPercentage').value
      );
      const computedAmount =
        totalAmount + totalAmount * 0.01 * deductionPercentage;

      this.form.get('totalLoanAmount').setValue(round(computedAmount, 2));
    };
  }
}
