import { TestBed } from '@angular/core/testing';

import { PaystubService } from './paystub.service';

describe('PaystubService', () => {
  let service: PaystubService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PaystubService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
