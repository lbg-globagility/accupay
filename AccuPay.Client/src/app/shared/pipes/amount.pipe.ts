import { Pipe, PipeTransform } from '@angular/core';
import { DecimalPipe } from '@angular/common';

@Pipe({
  name: 'amount',
})
export class AmountPipe extends DecimalPipe implements PipeTransform {
  transform(value: any, valueIfZero: string = null): string {
    if (typeof value !== 'number') {
      return value;
    }

    const num = value as number;

    if (valueIfZero && num == 0) {
      return valueIfZero;
    }

    const numberFormat: string = '1.2-2';
    const formatted = super.transform(Math.abs(num), numberFormat, null);

    return num < 0 ? `(${formatted})` : formatted;
  }
}
