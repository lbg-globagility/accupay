import { TestBed } from '@angular/core/testing';

import { AllowanceTypeService } from './allowance-type.service';

describe('AllowanceTypeService', () => {
  let service: AllowanceTypeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AllowanceTypeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
