import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Salary } from 'src/app/salaries/shared/salary';
import { SalaryService } from 'src/app/salaries/salary.service';

@Component({
  selector: 'app-view-salary',
  templateUrl: './view-salary.component.html',
  styleUrls: ['./view-salary.component.scss']
})
export class ViewSalaryComponent implements OnInit {

  salary: Salary;

  salaryId = this.route.snapshot.paramMap.get('id');

  constructor(
    private salaryService: SalaryService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.loadUser();
  }

  private loadUser() {
    this.salaryService.get(this.salaryId).subscribe((data) => {
      this.salary = data;
    });
  }

}
