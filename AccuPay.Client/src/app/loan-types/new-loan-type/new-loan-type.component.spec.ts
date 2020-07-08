import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewLoanTypeComponent } from './new-loan-type.component';

describe('NewLoanTypeComponent', () => {
  let component: NewLoanTypeComponent;
  let fixture: ComponentFixture<NewLoanTypeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewLoanTypeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewLoanTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
