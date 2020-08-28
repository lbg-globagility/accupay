import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfserveTimeEntryFormComponent } from './selfserve-time-entry-form.component';

describe('SelfserveTimeEntryFormComponent', () => {
  let component: SelfserveTimeEntryFormComponent;
  let fixture: ComponentFixture<SelfserveTimeEntryFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfserveTimeEntryFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfserveTimeEntryFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
