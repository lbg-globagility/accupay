import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { Role } from 'src/app/roles/shared/role';
import { PermissionService } from 'src/app/roles/services/permission.service';
import { Permission } from 'src/app/roles/shared/permission';

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
    this.loadPermissions();

    if (this.role) {
      this.form.patchValue(this.role);
    }
  }

  get valid(): boolean {
    this.form.markAllAsTouched();
    return this.form.valid;
  }

  get value(): Role {
    return this.form.value;
  }

  private loadPermissions() {
    this.permissionService.getAll().subscribe((permissions) => {
      this.permissions = permissions;
      this.addRolePermissions();
    });
  }

  private addRolePermissions() {
    this.permissions.forEach((p) => this.addRolePermission(p));
  }

  private addRolePermission(permission: Permission) {
    const rolePermission = this.role?.rolePermissions.find(
      (t) => t.permissionId === permission.id
    );

    this.rolePermissions.push(
      this.fb.group({
        name: [permission.name],
        permissionId: [permission.id],
        read: [rolePermission?.read ?? false],
        create: [rolePermission?.create ?? false],
        update: [rolePermission?.update ?? false],
        delete: [rolePermission?.delete ?? false],
      })
    );
  }
}
