import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfserveLeaveComponent } from './selfserve-leave.component';

describe('SelfserveLeaveComponent', () => {
  let component: SelfserveLeaveComponent;
  let fixture: ComponentFixture<SelfserveLeaveComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfserveLeaveComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfserveLeaveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
