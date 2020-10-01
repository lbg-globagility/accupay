import { TestBed } from '@angular/core/testing';

import { SelfserviceOfficialBusinessService } from './selfservice-official-business.service';

describe('SelfserviceOfficialBusinessService', () => {
  let service: SelfserviceOfficialBusinessService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SelfserviceOfficialBusinessService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
