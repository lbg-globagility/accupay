import { Component, Input } from '@angular/core';
import { Paystub } from '../shared/paystub';

@Component({
  selector: 'app-paystub-details',
  templateUrl: './paystub-details.component.html',
  styleUrls: ['./paystub-details.component.scss'],
})
export class PaystubDetailsComponent {
  @Input()
  paystub: Paystub;
}
