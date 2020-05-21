import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewAllowanceComponent } from './new-allowance.component';

describe('NewAllowanceComponent', () => {
  let component: NewAllowanceComponent;
  let fixture: ComponentFixture<NewAllowanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewAllowanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewAllowanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
