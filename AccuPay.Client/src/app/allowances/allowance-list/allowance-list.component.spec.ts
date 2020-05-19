import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllowanceListComponent } from './allowance-list.component';

describe('AllowanceListComponent', () => {
  let component: AllowanceListComponent;
  let fixture: ComponentFixture<AllowanceListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllowanceListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllowanceListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
