import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/users/user.service';
import { User } from 'src/app/users/shared/user';
import { Router } from '@angular/router';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-new-user',
  templateUrl: './new-user.component.html',
  styleUrls: ['./new-user.component.scss'],
})
export class NewUserComponent implements OnInit {
  constructor(
    private userService: UserService,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {}

  save(user: User) {
    this.userService.create(user, false).subscribe(
      (u) => {
        this.router.navigate(['users', u.id]);
      },
      (err) => this.errorHandler.badRequest(err, 'Failed to create user.')
    );
  }
}
