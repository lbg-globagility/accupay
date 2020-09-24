import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfserviceDashboardComponent } from './selfservice-dashboard.component';

describe('SelfserviceDashboardComponent', () => {
  let component: SelfserviceDashboardComponent;
  let fixture: ComponentFixture<SelfserviceDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfserviceDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfserviceDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
