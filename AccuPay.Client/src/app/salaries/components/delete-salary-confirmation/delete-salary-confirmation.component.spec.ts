import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteSalaryConfirmationComponent } from './delete-salary-confirmation.component';

describe('DeleteSalaryConfirmationComponent', () => {
  let component: DeleteSalaryConfirmationComponent;
  let fixture: ComponentFixture<DeleteSalaryConfirmationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeleteSalaryConfirmationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeleteSalaryConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
