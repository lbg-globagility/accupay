import { Pipe, PipeTransform } from '@angular/core';
import { DecimalPipe } from '@angular/common';

@Pipe({
  name: 'ifZero',
})
export class IfZeroPipe extends DecimalPipe implements PipeTransform {
  readonly emptyOutput = '-';

  transform(
    value: any,
    numberFormat: string = null,
    defaultIfEmpty: string = this.emptyOutput
  ): any {
    let input = Number(value);

    if (isNaN(input) || input == 0) {
      const alternative = !defaultIfEmpty ? this.emptyOutput : defaultIfEmpty;
      return alternative;
    } else {
      return `${super.transform(value, numberFormat, null)}`;
    }
  }
}
