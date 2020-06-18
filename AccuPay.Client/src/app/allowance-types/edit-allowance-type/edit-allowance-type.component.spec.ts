import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditAllowanceTypeComponent } from './edit-allowance-type.component';

describe('EditAllowanceTypeComponent', () => {
  let component: EditAllowanceTypeComponent;
  let fixture: ComponentFixture<EditAllowanceTypeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditAllowanceTypeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditAllowanceTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
