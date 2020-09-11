import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfServiceOvertimesComponent } from './self-service-overtimes.component';

describe('SelfServiceOvertimesComponent', () => {
  let component: SelfServiceOvertimesComponent;
  let fixture: ComponentFixture<SelfServiceOvertimesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfServiceOvertimesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfServiceOvertimesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
