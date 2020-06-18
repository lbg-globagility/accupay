import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpandableRowComponent } from './expandable-row.component';

describe('ExpandableRowComponent', () => {
  let component: ExpandableRowComponent;
  let fixture: ComponentFixture<ExpandableRowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExpandableRowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExpandableRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
