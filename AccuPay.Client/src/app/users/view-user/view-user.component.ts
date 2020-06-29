import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/accounts/services/account.service';
import { User } from 'src/app/users/shared/user';
import { UserService } from 'src/app/users/user.service';

@Component({
  selector: 'app-view-user',
  templateUrl: './view-user.component.html',
  styleUrls: ['./view-user.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class ViewUserComponent implements OnInit {
  user: User;

  imageUrl: string;

  userId = this.route.snapshot.paramMap.get('id');

  moment: string;
  organizationPictureUrl: string;

  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
    private accountService: AccountService
  ) {}

  ngOnInit() {
    this.moment = new Date().getTime().toString();
    this.loadUser();
    this.loadOrganization();
  }

  private loadUser() {
    this.userService.get(this.userId).subscribe((data) => {
      this.user = data;
      this.imageUrl = `/api/account/image/${this.user.id}?${this.moment}`;
    });
  }

  private loadOrganization() {
    this.accountService.getOrganization().subscribe((organization) => {
      this.organizationPictureUrl = `/api/account/image/org/${organization.id}?${this.moment}`;
    });
  }
}
