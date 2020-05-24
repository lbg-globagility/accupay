import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditOvertimeComponent } from './edit-overtime.component';

describe('EditOvertimeComponent', () => {
  let component: EditOvertimeComponent;
  let fixture: ComponentFixture<EditOvertimeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditOvertimeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditOvertimeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
