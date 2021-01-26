import { Component, Input, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-employee-avatar',
  templateUrl: './employee-avatar.component.html',
  styleUrls: ['./employee-avatar.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    class: 'block flex items-center',
  },
})
export class EmployeeAvatarComponent {
  @Input()
  fullName: string;

  @Input()
  employeeId: number;

  @Input()
  employeeNo: string;
}
