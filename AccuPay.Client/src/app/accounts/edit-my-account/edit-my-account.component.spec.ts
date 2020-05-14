import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditMyAccountComponent } from './edit-my-account.component';

describe('EditMyAccountComponent', () => {
  let component: EditMyAccountComponent;
  let fixture: ComponentFixture<EditMyAccountComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditMyAccountComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditMyAccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
