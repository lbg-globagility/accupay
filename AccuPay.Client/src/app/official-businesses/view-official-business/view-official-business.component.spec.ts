import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewOfficialBusinessComponent } from './view-official-business.component';

describe('ViewOfficialBusinessComponent', () => {
  let component: ViewOfficialBusinessComponent;
  let fixture: ComponentFixture<ViewOfficialBusinessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewOfficialBusinessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewOfficialBusinessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
