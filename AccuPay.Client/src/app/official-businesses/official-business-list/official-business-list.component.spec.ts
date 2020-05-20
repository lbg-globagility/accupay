import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OfficialBusinessListComponent } from './official-business-list.component';

describe('OfficialBusinessListComponent', () => {
  let component: OfficialBusinessListComponent;
  let fixture: ComponentFixture<OfficialBusinessListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OfficialBusinessListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OfficialBusinessListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
