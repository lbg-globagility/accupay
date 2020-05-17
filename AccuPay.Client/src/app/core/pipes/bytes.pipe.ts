import { Pipe, PipeTransform } from '@angular/core';
import * as prettyBytes from 'pretty-bytes';

@Pipe({
  name: 'bytes'
})
export class BytesPipe implements PipeTransform {
  transform(value: any, args?: any): any {
    return prettyBytes(value);
  }
}
