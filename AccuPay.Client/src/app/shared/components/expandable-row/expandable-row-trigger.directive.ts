import { Directive } from '@angular/core';

@Directive({
  selector: '[appExpandableRowTrigger]',
  host: {
    '[class]': "'app-expandable-row-trigger'",
  },
})
export class ExpandableRowTriggerDirective {
  constructor() {}
}
