import * as moment from 'moment';

export function range(dateFrom: Date, dateTo: Date): Date[] {
  const current = moment(dateFrom);
  const last = moment(dateTo).add(1, 'days');

  const dates: Date[] = [];

  while (current.isBefore(last)) {
    dates.push(current.toDate());
    current.add(1, 'days');
  }

  return dates;
}
