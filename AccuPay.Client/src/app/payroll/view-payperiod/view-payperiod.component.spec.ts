import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewPayperiodComponent } from './view-payperiod.component';

describe('ViewPayperiodComponent', () => {
  let component: ViewPayperiodComponent;
  let fixture: ComponentFixture<ViewPayperiodComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewPayperiodComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewPayperiodComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
