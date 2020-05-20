import { TestBed } from '@angular/core/testing';

import { OfficialBusinessService } from './official-business.service';

describe('OfficialBusinessService', () => {
  let service: OfficialBusinessService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OfficialBusinessService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
