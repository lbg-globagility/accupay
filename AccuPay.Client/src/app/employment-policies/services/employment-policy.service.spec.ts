import { TestBed } from '@angular/core/testing';

import { EmploymentPolicyService } from './employment-policy.service';

describe('EmploymentPolicyService', () => {
  let service: EmploymentPolicyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EmploymentPolicyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
