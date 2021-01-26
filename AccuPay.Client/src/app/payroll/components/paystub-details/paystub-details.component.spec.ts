import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaystubDetailsComponent } from './paystub-details.component';

describe('PaystubDetailsComponent', () => {
  let component: PaystubDetailsComponent;
  let fixture: ComponentFixture<PaystubDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PaystubDetailsComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaystubDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
