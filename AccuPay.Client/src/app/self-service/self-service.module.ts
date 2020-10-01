import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { SelfServeComponent } from './self-serve/self-serve.component';
import { SelfserveTimeEntryFormComponent } from './components/selfserve-time-entry-form/selfserve-time-entry-form.component';
import { LeavesModule } from '../leaves/leaves.module';
import { SelfserveTimeEntryComponent } from './pages/selfserve-time-entry/selfserve-time-entry.component';
import { TimeEntryModule } from '../time-entry/time-entry.module';
import { MatChipsModule } from '@angular/material/chips';
import { PayperiodSelectComponent } from './components/payperiod-select/payperiod-select.component';
import { TopbarComponent } from './components/topbar/topbar.component';
import { SelfServiceTimesheetsComponent } from './components/self-service-timesheets/self-service-timesheets.component';
import {
  SelfserviceLeaveFormComponent,
  SelfserviceLeavesComponent,
  SelfserviceNewLeaveComponent,
} from 'src/app/self-service/leaves/components';
import {
  SelfserviceNewOvertimeComponent,
  SelfserviceOvertimeFormComponent,
  SelfserviceOvertimesComponent,
} from 'src/app/self-service/overtimes/components';
import {
  SelfserviceNewOfficialBusinessComponent,
  SelfserviceOfficialBusinessesComponent,
  SelfserviceOfficialBusinessFormComponent,
} from 'src/app/self-service/official-businesses/components';
import { SelfserviceDashboardComponent } from './dashboard/components/selfservice-dashboard/selfservice-dashboard.component';

@NgModule({
  declarations: [
    SelfServeComponent,
    SelfserviceLeaveFormComponent,
    SelfserviceLeavesComponent,
    SelfserviceNewLeaveComponent,
    SelfserviceOvertimeFormComponent,
    SelfserveTimeEntryFormComponent,
    SelfserviceNewOvertimeComponent,
    SelfserveTimeEntryComponent,
    PayperiodSelectComponent,
    TopbarComponent,
    SelfserviceOvertimesComponent,
    SelfServiceTimesheetsComponent,
    SelfserviceOfficialBusinessesComponent,
    SelfserviceOfficialBusinessFormComponent,
    SelfserviceNewOfficialBusinessComponent,
    SelfserviceDashboardComponent,
  ],
  imports: [MatChipsModule, SharedModule, LeavesModule, TimeEntryModule],
})
export class SelfServiceModule {}
