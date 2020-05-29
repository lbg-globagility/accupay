import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewDivisionComponent } from './new-division.component';

describe('NewDivisionComponent', () => {
  let component: NewDivisionComponent;
  let fixture: ComponentFixture<NewDivisionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewDivisionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewDivisionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
