import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PositionFormComponent } from './position-form.component';

describe('PositionFormComponent', () => {
  let component: PositionFormComponent;
  let fixture: ComponentFixture<PositionFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PositionFormComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PositionFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
