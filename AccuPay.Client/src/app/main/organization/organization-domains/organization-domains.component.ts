import {
  Component,
  EventEmitter,
  OnInit,
  Output
  } from '@angular/core';
import { FileProgress } from 'src/app/files/shared/file-progress';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { Organization } from 'src/app/accounts/shared/organization';
import { OrganizationFormService } from 'src/app/main/organization/organization-form.service';

@Component({
  selector: 'app-organization-domains',
  templateUrl: './organization-domains.component.html',
  styleUrls: ['./organization-domains.component.scss']
})
export class OrganizationDomainsComponent implements OnInit {
  @Output()
  save: EventEmitter<any> = new EventEmitter<any>();

  currentProgress: FileProgress<Organization> = new FileProgress(
    'Not Started',
    0
  );

  constructor(
    private formService: OrganizationFormService,
    private fb: FormBuilder
  ) {}

  ngOnInit() {}

  get form(): FormGroup {
    return this.formService.form;
  }

  get domains(): FormArray {
    return this.form.get('domains') as FormArray;
  }

  removeDomain(index: number): void {
    this.domains.removeAt(index);
  }

  addDomain() {
    const domainGroup = this.createDomain({ id: null, name: '' });
    this.domains.push(domainGroup);
  }

  private createDomain(domain) {
    return this.fb.group({
      id: [domain.id],
      name: [domain.name]
    });
  }

  onSave() {
    if (!this.form.valid) {
      return;
    }

    this.save.emit();
  }
}
