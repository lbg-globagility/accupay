import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeAvatarComponent } from './employee-avatar.component';

describe('EmployeeAvatarComponent', () => {
  let component: EmployeeAvatarComponent;
  let fixture: ComponentFixture<EmployeeAvatarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeAvatarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeAvatarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
