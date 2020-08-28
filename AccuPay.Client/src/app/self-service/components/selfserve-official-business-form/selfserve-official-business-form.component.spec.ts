import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfserveOfficialBusinessFormComponent } from './selfserve-official-business-form.component';

describe('SelfserveOfficialBusinessFormComponent', () => {
  let component: SelfserveOfficialBusinessFormComponent;
  let fixture: ComponentFixture<SelfserveOfficialBusinessFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfserveOfficialBusinessFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfserveOfficialBusinessFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
