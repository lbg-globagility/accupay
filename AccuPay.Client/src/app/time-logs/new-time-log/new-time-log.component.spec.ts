import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewTimeLogComponent } from './new-time-log.component';

describe('NewTimeLogComponent', () => {
  let component: NewTimeLogComponent;
  let fixture: ComponentFixture<NewTimeLogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewTimeLogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewTimeLogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
