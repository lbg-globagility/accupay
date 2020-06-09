import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'amount',
})
export class AmountPipe implements PipeTransform {
  transform(value: any, ...args: any[]): string {
    const places = 2;

    if (typeof value !== 'number') {
      return value;
    }

    const n = value as number;
    const isNegative = n < 0;
    const present = Math.abs(n).toFixed(places);

    return isNegative ? `(${present})` : present;
  }
}
