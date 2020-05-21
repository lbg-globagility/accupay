import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewOvertimeComponent } from './new-overtime.component';

describe('NewOvertimeComponent', () => {
  let component: NewOvertimeComponent;
  let fixture: ComponentFixture<NewOvertimeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewOvertimeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewOvertimeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
