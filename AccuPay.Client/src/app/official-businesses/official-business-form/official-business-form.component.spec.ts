import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OfficialBusinessFormComponent } from './official-business-form.component';

describe('OfficialBusinessFormComponent', () => {
  let component: OfficialBusinessFormComponent;
  let fixture: ComponentFixture<OfficialBusinessFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OfficialBusinessFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OfficialBusinessFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
