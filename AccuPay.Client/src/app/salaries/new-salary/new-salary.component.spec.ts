import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewSalaryComponent } from './new-salary.component';

describe('NewSalaryComponent', () => {
  let component: NewSalaryComponent;
  let fixture: ComponentFixture<NewSalaryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [NewSalaryComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewSalaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
