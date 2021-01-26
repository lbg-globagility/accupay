import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllowanceFormComponent } from './allowance-form.component';

describe('AllowanceFormComponent', () => {
  let component: AllowanceFormComponent;
  let fixture: ComponentFixture<AllowanceFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllowanceFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllowanceFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
