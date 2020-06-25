import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Client } from 'src/app/clients/shared/client';

@Component({
  selector: 'app-client-form',
  templateUrl: './client-form.component.html',
  styleUrls: ['./client-form.component.scss'],
})
export class ClientFormComponent implements OnInit {
  @Input()
  client: Client;

  form: FormGroup = this.fb.group({
    name: [, [Validators.required]],
    tradeName: [],
    address: [],
    phoneNumber: [],
    contactPerson: [],
  });

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    if (this.client != null) {
      this.form.patchValue(this.client);
    }
  }

  get valid(): boolean {
    this.form.markAllAsTouched();
    return this.form.valid;
  }

  get value(): Client {
    return this.form.value;
  }
}
