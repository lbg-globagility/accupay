import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfserviceOvertimesComponent } from './selfservice-overtimes.component;

describe('SelfserviceOvertimesComponent', () => {
  let component: SelfserviceOvertimesComponent;
  let fixture: ComponentFixture<SelfserviceOvertimesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SelfserviceOvertimesComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfserviceOvertimesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
