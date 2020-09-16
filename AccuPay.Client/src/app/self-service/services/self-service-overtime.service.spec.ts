import { TestBed } from '@angular/core/testing';

import { SelfServiceOvertimeService } from './self-service-overtime.service';

describe('SelfServiceOvertimeService', () => {
  let service: SelfServiceOvertimeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SelfServiceOvertimeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
