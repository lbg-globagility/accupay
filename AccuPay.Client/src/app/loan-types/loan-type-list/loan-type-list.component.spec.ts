import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoanTypeListComponent } from './loan-type-list.component';

describe('LoanTypeListComponent', () => {
  let component: LoanTypeListComponent;
  let fixture: ComponentFixture<LoanTypeListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LoanTypeListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoanTypeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
