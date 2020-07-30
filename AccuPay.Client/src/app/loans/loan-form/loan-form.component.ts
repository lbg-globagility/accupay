import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Employee } from 'src/app/employees/shared/employee';
import { EmployeeService } from 'src/app/employees/services/employee.service';
import { Loan } from 'src/app/loans/shared/loan';
import { LoanService } from 'src/app/loans/loan.service';
import { SelectItem } from 'src/app/core/shared/select-item';
import { MatDialog } from '@angular/material/dialog';
import { NewLoanTypeComponent } from 'src/app/loan-types/new-loan-type/new-loan-type.component';
import { filter, startWith, map } from 'rxjs/operators';
import { Observable } from 'rxjs';

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
    status: [null, Validators.required],
    deductionSchedule: [null, Validators.required],
    comments: [null],
  });

  employees: Employee[];
  loanTypes: SelectItem[];
  statusList: string[];
  deductionSchedules: string[];

  isFormInitialized: boolean = false;

  filteredEmployees: Observable<Employee[]>;

  get valid(): boolean {
    this.form.markAllAsTouched();
    return this.form.valid;
  }

  get value(): any {
    return this.form.value;
  }

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private loanService: LoanService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadLoanTypes();
    this.loadLoanStatusList();
    this.loadDeductionSchedules();

    this.filteredEmployees = this.form.get('employeeId').valueChanges.pipe(
      startWith(''),
      map((value) => this._filter(value))
    );

    if (this.loan != null) {
      this.form.get('employeeId').disable();
      this.form.patchValue(this.loan);
    } else {
      this.loadEmployees();
    }

    this.isFormInitialized = true;
  }

  displayEmployee = (employeeId: number) => {
    const employee = this.employees?.find((t) => t.id === employeeId);
    if (employee == null) {
      return null;
    }

    return `${employee.employeeNo} - ${employee.lastName} ${employee.firstName}`;
  };

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

  newLoanType(): void {
    this.dialog
      .open(NewLoanTypeComponent)
      .afterClosed()
      .pipe(filter((t) => t))
      .subscribe(() => this.loadLoanTypes());
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

  private _filter(value: string) {
    if (typeof value !== 'string') {
      return;
    }

    const filterValue = value.toLowerCase();

    return this.employees?.filter(
      (employee) =>
        employee.employeeNo?.toLowerCase().includes(filterValue) ||
        employee.firstName?.toLowerCase().includes(filterValue) ||
        employee.lastName?.toLowerCase().includes(filterValue)
    );
  }
}
