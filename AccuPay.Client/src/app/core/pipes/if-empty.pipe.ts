import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'ifEmpty'
})
export class IfEmptyPipe implements PipeTransform {
  transform(value: any, ...args: any[]): any {
    if (value == null || value === '') {
      const alternative = args.length < 1 ? args[0] : '-';
      return alternative;
    } else {
      return value;
    }
  }
}
