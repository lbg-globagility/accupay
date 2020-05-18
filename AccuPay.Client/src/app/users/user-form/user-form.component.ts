import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from 'src/app/users/shared/user';

interface FormValue {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  title: string;
  phoneNumber: string;
  isUnregistered: boolean;
  role: string;
  recallCategories: string[];
  location: string;
}

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss'],
})
export class UserFormComponent implements OnInit {
  @Input()
  user: User;

  @Output()
  save: EventEmitter<User> = new EventEmitter();

  @Output()
  cancel: EventEmitter<any> = new EventEmitter();

  form: FormGroup = this.fb.group({
    id: [null],
    email: [null, [Validators.required, Validators.email]],
    firstName: [null, Validators.required],
    lastName: [null, Validators.required],
  });

  showUnregisteredUserOption: boolean = false;

  constructor(private fb: FormBuilder) {}

  private get value(): FormValue {
    return this.form.value;
  }

  ngOnInit(): void {
    if (this.user) {
      this.form.patchValue(this.user);
    }
  }

  onSave(): void {
    if (!this.form.valid) {
      return;
    }

    const user = this.form.value as User;

    this.save.emit(user);
  }

  onCancel(): void {
    this.cancel.emit();
  }
}
