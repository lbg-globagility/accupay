import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfServiceDashboardComponent } from './self-service-dashboard.component';

describe('SelfServiceDashboardComponent', () => {
  let component: SelfServiceDashboardComponent;
  let fixture: ComponentFixture<SelfServiceDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfServiceDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfServiceDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
