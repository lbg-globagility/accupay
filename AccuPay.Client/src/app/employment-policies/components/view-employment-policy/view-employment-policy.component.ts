import { Component, OnInit } from '@angular/core';
import { EmploymentPolicy } from 'src/app/employment-policies/shared';
import { EmploymentPolicyService } from 'src/app/employment-policies/services/employment-policy.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-view-employment-policy',
  templateUrl: './view-employment-policy.component.html',
  styleUrls: ['./view-employment-policy.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class ViewEmploymentPolicyComponent implements OnInit {
  employmentPolicyId: number = +this.route.snapshot.paramMap.get('id');

  employmentPolicy: EmploymentPolicy;

  constructor(
    private employmentPolicyService: EmploymentPolicyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadEmploymentPolicy();
  }

  private loadEmploymentPolicy(): void {
    this.employmentPolicyService
      .getById(this.employmentPolicyId)
      .subscribe(
        (employmentPolicy) => (this.employmentPolicy = employmentPolicy)
      );
  }
}
