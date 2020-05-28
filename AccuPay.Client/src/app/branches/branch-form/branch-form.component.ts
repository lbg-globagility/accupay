import { Component, OnInit, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Branch } from 'src/app/branches/shared/branch';

@Component({
  selector: 'app-branch-form',
  templateUrl: './branch-form.component.html',
  styleUrls: ['./branch-form.component.scss'],
})
export class BranchFormComponent implements OnInit {
  @Input()
  branch: Branch;

  @Output()
  save: EventEmitter<Branch> = new EventEmitter();

  @Output()
  cancel: EventEmitter<void> = new EventEmitter();

  form: FormGroup = this.fb.group({
    name: [, [Validators.required]],
    code: [, [Validators.required]],
  });

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    if (this.branch != null) {
      this.form.patchValue(this.branch);
    }
  }

  onSave() {
    if (!this.form.valid) {
      return;
    }

    const branch = this.form.value;
    this.save.emit(branch);
  }

  onCancel() {
    this.cancel.emit();
  }
}
