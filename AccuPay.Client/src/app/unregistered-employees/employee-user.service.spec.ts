import { TestBed } from '@angular/core/testing';

import { EmployeeUserService } from './employee-user.service';

describe('EmployeeUserService', () => {
  let service: EmployeeUserService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EmployeeUserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
