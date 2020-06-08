import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'ifZero',
})
export class IfZeroPipe implements PipeTransform {
  transform(value: number, ...args: any[]): any {
    if (value == null || value == 0) {
      const alternative = args.length > 0 ? args[0] : '-';
      return alternative;
    } else {
      return value;
    }
  }
}
