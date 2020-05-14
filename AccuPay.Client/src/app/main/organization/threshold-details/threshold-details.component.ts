import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  OnChanges
} from '@angular/core';
import { FormArray, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-threshold-details',
  templateUrl: './threshold-details.component.html',
  styleUrls: ['./threshold-details.component.scss']
})
export class ThresholdDetailsComponent {
  @Input() group: FormGroup;

  @Input() title: string;

  @Output()
  save: EventEmitter<any> = new EventEmitter<any>();

  get thresholdArray(): FormArray {
    return this.group.get('items') as FormArray;
  }

  getDescription(index: number): string {
    return this.thresholdArray.at(index).get('classification').value;
  }

  changefirst(index: number) {
    this.thresholdArray
      .at(index)
      .get('second')
      .setValue(Number(this.thresholdArray.at(index).get('first').value) + 2);
  }

  getYellowStart(index: number): number {
    const value = Number(this.thresholdArray.at(index).get('first').value);
    return value + 1;
  }

  getRedStart(index: number): number {
    const value = Number(this.thresholdArray.at(index).get('second').value);
    return value + 1;
  }

  onSave() {
    if (!this.group.valid) {
      return;
    }

    this.save.emit();
  }
}
