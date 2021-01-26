import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { Role } from 'src/app/roles/shared/role';
import { PermissionService } from 'src/app/roles/services/permission.service';
import { Permission } from 'src/app/roles/shared/permission';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-role-form',
  templateUrl: './role-form.component.html',
  styleUrls: ['./role-form.component.scss'],
})
export class RoleFormComponent implements OnInit {
  @Input()
  role: Role;

  form: FormGroup = this.fb.group({
    name: [, [Validators.required]],
    rolePermissions: this.fb.array([]),
  });

  permissions: Permission[];

  get rolePermissions(): FormArray {
    return this.form.get('rolePermissions') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private permissionService: PermissionService
  ) {}

  ngOnInit(): void {
    this.loadPermissions().subscribe(() => {
      if (this.role) {
        this.patchValue();
      }
    });
  }

  get valid(): boolean {
    this.form.markAllAsTouched();
    return this.form.valid;
  }

  get value(): Role {
    return this.form.value;
  }

  private loadPermissions(): Observable<void> {
    return this.permissionService.getAll().pipe(
      map((permissions) => {
        this.permissions = permissions;
        this.permissions.forEach((p) => this.addRolePermission(p));
      })
    );
  }

  private addRolePermission(permission: Permission) {
    this.rolePermissions.push(
      this.fb.group({
        name: [permission.name],
        permissionId: [permission.id],
        read: [false],
        create: [false],
        update: [false],
        delete: [false],
      })
    );
  }

  private patchValue() {
    this.form.patchValue(this.role);

    if (this.role.isAdmin) {
      this.form.disable();
    }
  }
}
