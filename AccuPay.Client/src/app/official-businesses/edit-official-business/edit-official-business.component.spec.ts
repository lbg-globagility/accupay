import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditOfficialBusinessComponent } from './edit-official-business.component';

describe('EditOfficialBusinessComponent', () => {
  let component: EditOfficialBusinessComponent;
  let fixture: ComponentFixture<EditOfficialBusinessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditOfficialBusinessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditOfficialBusinessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
