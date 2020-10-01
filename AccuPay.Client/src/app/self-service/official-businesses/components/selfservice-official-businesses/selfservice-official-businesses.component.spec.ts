import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfserviceOfficialBusinessesComponent } from './selfservice-official-businesses.component';

describe('SelfserviceOfficialBusinessesComponent', () => {
  let component: SelfserviceOfficialBusinessesComponent;
  let fixture: ComponentFixture<SelfserviceOfficialBusinessesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfserviceOfficialBusinessesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfserviceOfficialBusinessesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
