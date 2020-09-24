import { TestBed } from '@angular/core/testing';

import { SelfServiceLeaveService } from './self-service-leave.service';

describe('SelfServiceLeaveService', () => {
  let service: SelfServiceLeaveService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SelfServiceLeaveService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
