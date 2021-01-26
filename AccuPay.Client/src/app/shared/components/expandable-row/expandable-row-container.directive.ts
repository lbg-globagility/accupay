import { Directive } from '@angular/core';

@Directive({
  selector: '[appExpandableRowContainer]',
  host: {
    '[class]': "'app-expandable-row-container'",
  },
})
export class ExpandableRowContainerDirective {
  constructor() {}
}
