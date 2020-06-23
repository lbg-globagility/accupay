import { Component, OnInit } from '@angular/core';
import { BranchService } from 'src/app/branches/services/branch.service';
import { Branch } from 'src/app/branches/shared/branch';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-branch-list',
  templateUrl: './branch-list.component.html',
  styleUrls: ['./branch-list.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class BranchListComponent implements OnInit {
  readonly displayedColumns: string[] = ['name', 'code', 'actions'];

  dataSource: MatTableDataSource<Branch>;

  constructor(private branchService: BranchService) {}

  ngOnInit(): void {
    this.loadBranches();
  }

  loadBranches() {
    this.branchService.list().subscribe((branches) => {
      this.dataSource = new MatTableDataSource(branches);
    });
  }
}
