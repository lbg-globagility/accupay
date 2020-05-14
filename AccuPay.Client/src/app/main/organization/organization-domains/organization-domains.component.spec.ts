import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrganizationDomainsComponent } from './organization-domains.component';

describe('OrganizationDomainsComponent', () => {
  let component: OrganizationDomainsComponent;
  let fixture: ComponentFixture<OrganizationDomainsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrganizationDomainsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrganizationDomainsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
