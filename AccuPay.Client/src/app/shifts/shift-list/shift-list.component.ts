import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { Shift } from '../shared/shift';
import { MatTableDataSource } from '@angular/material/table';
import { Sort } from '@angular/material/sort';
import { ShiftService } from '../shift.service';
import { auditTime } from 'rxjs/operators';
import { Constants } from 'src/app/core/shared/constants';
import { PageOptions } from 'src/app/core/shared/page-options';
import { PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-shift-list',
  templateUrl: './shift-list.component.html',
  styleUrls: ['./shift-list.component.scss']
})
export class ShiftListComponent implements OnInit {
  readonly displayedColumns: string[] = [
    'employeeNumber',
    'employeeName',
    'date',
    'time',
    'breakTime',
    'breakLength'
  ];

  placeholder: string;

  searchTerm: string;

  modelChanged: Subject<any>;

  shifts: Shift[];

  totalCount: number;

  dataSource: MatTableDataSource<Shift>;

  pageIndex = 0;

  pageSize: number = 10;

  sort: Sort = {
    active: 'employeeNumber',
    direction: '',
  };

  clearSearch = '';

  selectedRow: number;

  constructor(
    private shiftService: ShiftService,    
    private router: Router,
    private snackBar: MatSnackBar) {
    this.modelChanged = new Subject();
    this.modelChanged
      .pipe(auditTime(Constants.ThrottleTime))
      .subscribe(() => this.getOvertimeList());
  }

  ngOnInit(): void {
    this.getOvertimeList();
  }

  getOvertimeList() {
    const options = new PageOptions(
      this.pageIndex,
      this.pageSize,
      this.sort.active,
      this.sort.direction
    );

    this.shiftService.getAll(options, this.searchTerm).subscribe((data) => {
      this.shifts = data.items;
      this.totalCount = data.totalCount;
      this.dataSource = new MatTableDataSource(this.shifts);
    });
  }

  applyFilter(searchTerm: string) {
    this.searchTerm = searchTerm;
    this.pageIndex = 0;
    this.modelChanged.next();
  }

  clearSearchBox() {
    this.clearSearch = '';
    this.applyFilter(this.clearSearch);
  }

  sortData(sort: Sort) {
    this.sort = sort;
    this.modelChanged.next();
  }

  setHoveredRow(id: number) {
    this.selectedRow = id;
  }

  onPageChanged(pageEvent: PageEvent) {
    this.pageIndex = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.getOvertimeList();
  }

  onImport(files: FileList){
    const file = files[0];

    this.shiftService.import(file).subscribe(
      () => {
        this.displaySuccess();
        this.router.navigate(['shifts']);
      },
      (err) => this.showErrorDialog(err)
    );
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully imported a new shift!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }

  private showErrorDialog(err): void {
    let message: string = 'Failed to import shift.';

    if (err && err.status == 400) {
      message = err.error.Error;
    }
    this.snackBar.open(message, null, {
      duration: 2000,
      panelClass: ['mat-toolbar', 'mat-warn'],
    });
  }
}
