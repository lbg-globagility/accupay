import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { SelfserviceNewLeaveComponent } from './selfservice-new-leave.component';

describe('SelfserviceNewLeaveComponent', () => {
  let component: SelfserviceNewLeaveComponent;
  let fixture: ComponentFixture<SelfserviceNewLeaveComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SelfserviceNewLeaveComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfserviceNewLeaveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
