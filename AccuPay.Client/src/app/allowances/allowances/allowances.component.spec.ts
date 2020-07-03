import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllowancesComponent } from './allowances.component';

describe('AllowancesComponent', () => {
  let component: AllowancesComponent;
  let fixture: ComponentFixture<AllowancesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllowancesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllowancesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
