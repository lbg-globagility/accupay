import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfServeComponent } from './self-serve.component';

describe('SelfServeComponent', () => {
  let component: SelfServeComponent;
  let fixture: ComponentFixture<SelfServeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfServeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfServeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
