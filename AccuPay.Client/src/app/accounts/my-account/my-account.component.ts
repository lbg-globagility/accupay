import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';
import { Account } from '../shared/account';

@Component({
  selector: 'app-my-account',
  templateUrl: './my-account.component.html',
  styleUrls: ['./my-account.component.scss']
})
export class MyAccountComponent implements OnInit {
  detail: Account;

  imageUrl: string;

  organizationPictureUrl: string;

  moment: string;

  constructor(private accountService: AccountService) {}

  ngOnInit(): void {
    this.moment = new Date().getTime().toString();
    this.loadUser();
    this.loadOrganization();
  }

  private loadOrganization() {
    this.accountService.getOrganization().subscribe(organization => {
      this.organizationPictureUrl = `/api/account/image/org/${organization.id}?${this.moment}`;
    });
  }

  private loadUser() {
    this.accountService.get().subscribe(data => {
      setTimeout(() => {
        this.detail = data;
        if (data.supplierId) {
          this.detail.companyId = data.supplierId;
        } else {
          this.detail.companyId = data.providerId;
        }
        this.imageUrl = `/api/account/image/${this.detail.id}?${this.moment}`;
      });
    });
  }
}
