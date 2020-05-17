import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ThresholdDetailsComponent } from './threshold-details.component';

describe('ThresholdDetailsComponent', () => {
  let component: ThresholdDetailsComponent;
  let fixture: ComponentFixture<ThresholdDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ThresholdDetailsComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ThresholdDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
