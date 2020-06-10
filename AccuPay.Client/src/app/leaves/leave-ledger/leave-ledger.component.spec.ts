import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LeaveLedgerComponent } from './leave-ledger.component';

describe('LeaveLedgerComponent', () => {
  let component: LeaveLedgerComponent;
  let fixture: ComponentFixture<LeaveLedgerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LeaveLedgerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LeaveLedgerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
