import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/users/user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/users/shared/user';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.scss'],
})
export class EditUserComponent implements OnInit {
  user: User;

  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const userId = this.route.snapshot.paramMap.get('id');
    this.userService.get(userId).subscribe((u) => (this.user = u));
  }

  save(user: User) {
    this.userService.update(user, this.user.id).subscribe((u) => {
      this.router.navigate(['users', u.id]);
    });
  }

  cancel() {
    this.router.navigate(['users', this.user.id]);
  }
}
