import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewOfficialBusinessComponent } from './new-official-business.component';

describe('NewOfficialBusinessComponent', () => {
  let component: NewOfficialBusinessComponent;
  let fixture: ComponentFixture<NewOfficialBusinessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewOfficialBusinessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewOfficialBusinessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
