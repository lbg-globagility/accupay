import { TestBed } from '@angular/core/testing';

import { OrganizationFormService } from './organization-form.service';

describe('OrganizationFormService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: OrganizationFormService = TestBed.get(OrganizationFormService);
    expect(service).toBeTruthy();
  });
});
