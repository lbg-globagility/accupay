import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllowanceTypeListComponent } from './allowance-type-list.component';

describe('AllowanceTypeListComponent', () => {
  let component: AllowanceTypeListComponent;
  let fixture: ComponentFixture<AllowanceTypeListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllowanceTypeListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllowanceTypeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
