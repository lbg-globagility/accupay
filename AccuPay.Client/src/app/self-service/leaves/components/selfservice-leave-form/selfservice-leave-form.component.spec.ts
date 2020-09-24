import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { SelfserviceLeaveFormComponent } from './selfservice-leave-form.component';

describe('SelfserviceLeaveFormComponent', () => {
  let component: SelfserviceLeaveFormComponent;
  let fixture: ComponentFixture<SelfserviceLeaveFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SelfserviceLeaveFormComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfserviceLeaveFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
