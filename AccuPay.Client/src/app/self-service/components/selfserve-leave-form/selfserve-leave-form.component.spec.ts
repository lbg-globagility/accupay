import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfserveLeaveFormComponent } from './selfserve-leave-form.component';

describe('SelfserveLeaveFormComponent', () => {
  let component: SelfserveLeaveFormComponent;
  let fixture: ComponentFixture<SelfserveLeaveFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfserveLeaveFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfserveLeaveFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
