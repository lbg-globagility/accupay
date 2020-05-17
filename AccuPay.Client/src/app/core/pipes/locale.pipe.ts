import * as moment from 'moment';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'locale'
})
export class LocalePipe implements PipeTransform {
  transform(value: any, args?: any): any {
    if (value == null) {
      return null;
    }

    return moment
      .utc(value)
      .local()
      .format('MMM DD, YYYY hh:mm a');
  }
}
