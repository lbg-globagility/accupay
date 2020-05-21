import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewAllowanceComponent } from './view-allowance.component';

describe('ViewAllowanceComponent', () => {
  let component: ViewAllowanceComponent;
  let fixture: ComponentFixture<ViewAllowanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ViewAllowanceComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewAllowanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
