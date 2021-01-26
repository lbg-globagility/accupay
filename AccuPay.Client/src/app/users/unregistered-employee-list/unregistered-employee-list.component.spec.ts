import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UnregisteredEmployeeListComponent } from './unregistered-employee-list.component';

describe('UnregisteredEmployeeListComponent', () => {
  let component: UnregisteredEmployeeListComponent;
  let fixture: ComponentFixture<UnregisteredEmployeeListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UnregisteredEmployeeListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UnregisteredEmployeeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
