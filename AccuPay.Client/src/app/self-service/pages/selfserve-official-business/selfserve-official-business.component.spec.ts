import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfserveOfficialBusinessComponent } from './selfserve-official-business.component';

describe('SelfserveOfficialBusinessComponent', () => {
  let component: SelfserveOfficialBusinessComponent;
  let fixture: ComponentFixture<SelfserveOfficialBusinessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfserveOfficialBusinessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfserveOfficialBusinessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
