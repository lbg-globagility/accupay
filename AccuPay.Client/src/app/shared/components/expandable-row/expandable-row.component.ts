import { Component, OnInit, Input, HostBinding } from '@angular/core';
import {
  state,
  transition,
  trigger,
  style,
  animate,
} from '@angular/animations';

@Component({
  selector: '[app-expandable-row]',
  templateUrl: './expandable-row.component.html',
  styleUrls: ['./expandable-row.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition(
        'expanded <=> collapsed',
        animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')
      ),
    ]),
  ],
})
export class ExpandableRowComponent {
  @HostBinding('@detailExpand')
  detailExpand: string = 'collapsed';

  @Input()
  set expanded(value: boolean) {
    this.detailExpand = value ? 'expanded' : 'collapsed';
  }
}
