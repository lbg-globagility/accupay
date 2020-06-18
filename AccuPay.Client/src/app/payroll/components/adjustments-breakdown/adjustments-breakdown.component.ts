import { Component, OnInit } from '@angular/core';
import { PaystubService } from '../../services/paystub.service';

@Component({
  selector: 'app-adjustments-breakdown',
  templateUrl: './adjustments-breakdown.component.html',
  styleUrls: ['./adjustments-breakdown.component.scss'],
})
export class AdjustmentsBreakdownComponent implements OnInit {
  constructor(private paystubService: PaystubService) {}

  ngOnInit(): void {}
}
