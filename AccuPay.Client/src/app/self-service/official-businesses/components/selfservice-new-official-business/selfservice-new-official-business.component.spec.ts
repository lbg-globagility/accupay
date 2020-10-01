import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfserviceNewOfficialBusinessComponent } from './selfservice-new-official-business.component';

describe('SelfserviceNewOfficialBusinessComponent', () => {
  let component: SelfserviceNewOfficialBusinessComponent;
  let fixture: ComponentFixture<SelfserviceNewOfficialBusinessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfserviceNewOfficialBusinessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfserviceNewOfficialBusinessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
