import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Position } from 'src/app/positions/shared/position';
import { Division } from 'src/app/divisions/shared/division';
import { DivisionService } from 'src/app/divisions/division.service';

@Component({
  selector: 'app-position-form',
  templateUrl: './position-form.component.html',
  styleUrls: ['./position-form.component.scss'],
})
export class PositionFormComponent implements OnInit {
  @Input()
  position: Position;

  @Output()
  save: EventEmitter<Position> = new EventEmitter();

  @Output()
  cancel: EventEmitter<void> = new EventEmitter();

  form: FormGroup = this.fb.group({
    divisionId: [null, [Validators.required]],
    name: [null, [Validators.required]],
  });

  divisions: Division[];

  constructor(
    private fb: FormBuilder,
    private divisionService: DivisionService
  ) {}

  ngOnInit(): void {
    this.loadEmployees();

    if (this.position) {
      this.form.patchValue(this.position);
    }
  }

  private loadEmployees(): void {
    this.divisionService.getAll().subscribe((data) => {
      this.divisions = data;
    });
  }

  onSave() {
    if (!this.form.valid) {
      return;
    }

    const position = this.form.value;
    this.save.emit(position);
  }

  onCancel() {
    this.cancel.emit();
  }
}
