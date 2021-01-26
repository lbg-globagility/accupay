import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { SelfserviceLeavesComponent } from './selfservice-leaves.component';

describe('SelfserviceLeavesComponent', () => {
  let component: SelfserviceLeavesComponent;
  let fixture: ComponentFixture<SelfserviceLeavesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SelfserviceLeavesComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfserviceLeavesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
