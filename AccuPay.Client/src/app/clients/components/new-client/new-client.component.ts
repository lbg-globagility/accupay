import { Component, OnInit, ViewChild } from '@angular/core';
import { ClientFormComponent } from 'src/app/clients/components/client-form/client-form.component';
import { ClientService } from 'src/app/clients/services/client.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { ErrorHandler } from 'src/app/core/shared/services/error-handler';

@Component({
  selector: 'app-new-client',
  templateUrl: './new-client.component.html',
  styleUrls: ['./new-client.component.scss'],
  host: {
    class: 'block p-4',
  },
})
export class NewClientComponent implements OnInit {
  @ViewChild(ClientFormComponent)
  clientForm: ClientFormComponent;

  constructor(
    private clientService: ClientService,
    private router: Router,
    private errorHandler: ErrorHandler
  ) {}

  ngOnInit(): void {}

  save() {
    if (!this.clientForm.valid) {
      return;
    }

    const value = this.clientForm.value;
    this.clientService.create(value).subscribe({
      next: (client) => {
        this.router.navigate(['clients', client.id]);
        this.displaySuccess();
      },
      error: (err) =>
        this.errorHandler.badRequest(err, 'Failed to save new client'),
    });
  }

  cancel() {
    this.router.navigate(['clients']);
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
