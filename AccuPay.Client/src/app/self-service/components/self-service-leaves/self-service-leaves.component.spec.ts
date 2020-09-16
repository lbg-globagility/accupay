import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfServiceLeavesComponent } from './self-service-leaves.component';

describe('SelfServiceLeavesComponent', () => {
  let component: SelfServiceLeavesComponent;
  let fixture: ComponentFixture<SelfServiceLeavesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfServiceLeavesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfServiceLeavesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
