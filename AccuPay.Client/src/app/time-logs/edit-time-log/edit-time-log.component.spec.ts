import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditTimeLogComponent } from './edit-time-log.component';

describe('EditTimeLogComponent', () => {
  let component: EditTimeLogComponent;
  let fixture: ComponentFixture<EditTimeLogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditTimeLogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditTimeLogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
