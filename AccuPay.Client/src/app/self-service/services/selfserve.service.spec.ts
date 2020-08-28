import { TestBed } from '@angular/core/testing';

import { SelfserveService } from './selfserve.service';

describe('SelfserveService', () => {
  let service: SelfserveService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SelfserveService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
