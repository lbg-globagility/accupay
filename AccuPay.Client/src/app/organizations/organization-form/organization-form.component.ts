import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Organization } from 'src/app/organizations/shared/organization';

@Component({
  selector: 'app-organization-form',
  templateUrl: './organization-form.component.html',
  styleUrls: ['./organization-form.component.scss'],
})
export class OrganizationFormComponent implements OnInit {
  @Input()
  organization: Organization;

  @Output()
  save: EventEmitter<Organization> = new EventEmitter();

  @Output()
  cancel: EventEmitter<void> = new EventEmitter();

  form: FormGroup = this.fb.group({
    name: [],
  });

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    if (this.organization) {
      this.form.patchValue(this.organization);
    }
  }

  onSave() {
    if (!this.form.valid) {
      return;
    }

    const organization = this.form.value;
    this.save.emit(organization);
  }

  onCancel() {
    this.cancel.emit();
  }
}
