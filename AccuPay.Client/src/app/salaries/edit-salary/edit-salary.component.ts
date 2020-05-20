import Swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { Salary } from 'src/app/salaries/shared/salary';
import { SalaryService } from 'src/app/salaries/salary.service';

@Component({
  selector: 'app-edit-salary',
  templateUrl: './edit-salary.component.html',
  styleUrls: ['./edit-salary.component.scss'],
})
export class EditSalaryComponent implements OnInit {
  salary: Salary;

  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(false);

  salaryId = this.route.snapshot.paramMap.get('id');

  constructor(
    private salaryService: SalaryService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadSalary();
  }

  onSave(salary: Salary) {
    console.log(salary);
    this.salaryService.update(salary, this.salaryId).subscribe(
      () => {
        this.displaySuccess();
        this.router.navigate(['salaries', this.salaryId]);
      },
      (err) => {}
    );
  }

  onCancel() {
    this.router.navigate(['salaries', this.salaryId]);
  }

  private loadSalary() {
    this.salaryService.get(this.salaryId).subscribe((data) => {
      this.salary = data;

      this.isLoading.next(true);
    });
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Complete',
      text: 'Successfully saved!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
