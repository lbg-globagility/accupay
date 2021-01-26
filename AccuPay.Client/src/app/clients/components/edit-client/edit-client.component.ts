import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ClientService } from 'src/app/clients/services/client.service';
import { Client } from 'src/app/clients/shared/client';
import { ClientFormComponent } from 'src/app/clients/components/client-form/client-form.component';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-edit-client',
  templateUrl: './edit-client.component.html',
  styleUrls: ['./edit-client.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class EditClientComponent implements OnInit {
  @ViewChild(ClientFormComponent)
  clientForm: ClientFormComponent;

  private clientId: number = +this.route.snapshot.paramMap.get('id');

  client: Client;

  constructor(
    private clientService: ClientService,
    private router: Router,
    private route: ActivatedRoute,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {
    this.clientService
      .getById(this.clientId)
      .subscribe((client) => (this.client = client));
  }

  save() {
    if (!this.clientForm.valid) {
      return;
    }

    const value = this.clientForm.value;
    this.clientService.update(this.clientId, value).subscribe({
      next: (client) => {
        this.router.navigate(['clients', client.id]);
        this.displaySuccess();
      },
      error: (err) =>
        this.errorHandler.badRequest(err, 'Failed to save new client'),
    });
  }

  cancel() {
    this.router.navigate(['clients', this.clientId]);
  }

  private displaySuccess() {
    Swal.fire({
      title: 'Success',
      text: 'Successfully created a new leave!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false,
    });
  }
}
