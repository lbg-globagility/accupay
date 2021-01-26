import { Pipe, PipeTransform } from '@angular/core';
import { DecimalPipe } from '@angular/common';

@Pipe({
  name: 'ifZero',
})
export class IfZeroPipe extends DecimalPipe implements PipeTransform {
  transform(
    value: any,
    numberFormat: string = null,
    valueIfZero: string = '-',
    locale: string = null
  ): string {
    if (typeof value !== 'number') {
      return value;
    }

    const num = value as number;

    if (num == 0) {
      return valueIfZero;
    }

    return `${super.transform(num, numberFormat, locale)}`;
  }
}
