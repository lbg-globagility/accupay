import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfserveTimeEntryComponent } from './selfserve-time-entry.component';

describe('SelfserveTimeEntryComponent', () => {
  let component: SelfserveTimeEntryComponent;
  let fixture: ComponentFixture<SelfserveTimeEntryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfserveTimeEntryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfserveTimeEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
