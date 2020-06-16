import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'ifZero',
})
export class IfZeroPipe implements PipeTransform {
  transform(value: any, ...args: any[]): any {
    let input = Number(value);

    if (isNaN(input) || input == 0) {
      const alternative = args.length > 0 ? args[0] : '-';
      return alternative;
    } else {
      return input;
    }
  }
}
