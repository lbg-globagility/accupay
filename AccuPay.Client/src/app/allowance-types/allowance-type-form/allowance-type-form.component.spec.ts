import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllowanceTypeFormComponent } from './allowance-type-form.component';

describe('AllowanceTypeFormComponent', () => {
  let component: AllowanceTypeFormComponent;
  let fixture: ComponentFixture<AllowanceTypeFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllowanceTypeFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllowanceTypeFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
