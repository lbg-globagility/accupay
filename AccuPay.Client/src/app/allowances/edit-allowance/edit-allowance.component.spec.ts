import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditAllowanceComponent } from './edit-allowance.component';

describe('EditAllowanceComponent', () => {
  let component: EditAllowanceComponent;
  let fixture: ComponentFixture<EditAllowanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditAllowanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditAllowanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
