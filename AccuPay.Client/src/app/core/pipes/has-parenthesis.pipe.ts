import { Pipe, PipeTransform } from '@angular/core';
import { IfZeroPipe } from './if-zero.pipe';

@Pipe({
  name: 'hasParenthesis',
})
export class HasParenthesisPipe extends IfZeroPipe implements PipeTransform {
  transform(
    value: any,
    numberFormat: string = '1.2-2',
    defaultIfEmpty: string = this.emptyOutput
  ): any {
    let output = super.transform(value, numberFormat, defaultIfEmpty);

    if (output == this.emptyOutput) return output;

    return `(${output})`;
  }
}
