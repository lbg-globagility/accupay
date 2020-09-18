import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfserviceOfficialBusinessFormComponent } from './selfservice-official-business-form.component';

describe('SelfserviceOfficialBusinessFormComponent', () => {
  let component: SelfserviceOfficialBusinessFormComponent;
  let fixture: ComponentFixture<SelfserviceOfficialBusinessFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfserviceOfficialBusinessFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfserviceOfficialBusinessFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
