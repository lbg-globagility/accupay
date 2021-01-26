import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewAllowanceTypeComponent } from './new-allowance-type.component';

describe('NewAllowanceTypeComponent', () => {
  let component: NewAllowanceTypeComponent;
  let fixture: ComponentFixture<NewAllowanceTypeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewAllowanceTypeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewAllowanceTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
