import { MenuItem } from 'src/app/main/shared';

export const selfServiceMenu: MenuItem[] = [
  {
    label: 'Dashboard',
    route: '/self-service',
  },
  {
    label: 'Leaves',
    route: '/self-service/leaves',
  },
  {
    label: 'Overtimes',
    route: '/self-service/overtimes',
  },
  {
    label: 'Official Businesses',
    route: '/self-service/official-businesses',
  },
  {
    label: 'Timesheets',
    route: '/self-service/timesheets',
  },
];
