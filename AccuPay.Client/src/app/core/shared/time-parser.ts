import * as moment from 'moment';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TimeParser {
  /**
   * This is the foo function
   * @date Date This should be a moment object in UTC. The method accepts it as Date but it is actually a moment object from mat-datepicker.
   * @time Date This is the value of the input=[time]. This should be 5 characters only with 2 digits each for hours and minutes separated by colon (:).
   * @returns returns the Date object of the combined values of @date and @time in UTC.
   */
  parse(date: moment.Moment, time: Date): Date {
    if (!time) return null;

    return new Date(date.format('L') + ' ' + time + ':00Z');
  }

  toInputTime(input: string): string {
    if (!input) return null;

    const date = new Date(input);
    return (
      this.twoDigits(date.getHours()) + ':' + this.twoDigits(date.getMinutes())
    );
  }

  private twoDigits(twoDigits: number) {
    return twoDigits > 9 ? '' + twoDigits : '0' + twoDigits;
  }
}
