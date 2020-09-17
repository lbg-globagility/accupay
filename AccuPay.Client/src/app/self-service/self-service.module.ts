import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { SelfServeComponent } from './self-serve/self-serve.component';
import { SelfserveOfficialBusinessFormComponent } from './components/selfserve-official-business-form/selfserve-official-business-form.component';
import { SelfserveTimeEntryFormComponent } from './components/selfserve-time-entry-form/selfserve-time-entry-form.component';
import { LeavesModule } from '../leaves/leaves.module';
import { SelfserveOfficialBusinessComponent } from './pages/selfserve-official-business/selfserve-official-business.component';
import { SelfserveTimeEntryComponent } from './pages/selfserve-time-entry/selfserve-time-entry.component';
import { TimeEntryModule } from '../time-entry/time-entry.module';
import { MatChipsModule } from '@angular/material/chips';
import { PayperiodSelectComponent } from './components/payperiod-select/payperiod-select.component';
import { TopbarComponent } from './components/topbar/topbar.component';
import { SelfServiceDashboardComponent } from './components/self-service-dashboard/self-service-dashboard.component';
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

@NgModule({
  declarations: [
    SelfServeComponent,
    SelfserviceLeaveFormComponent,
    SelfserviceLeavesComponent,
    SelfserviceNewLeaveComponent,
    SelfserviceOvertimeFormComponent,
    SelfserveOfficialBusinessFormComponent,
    SelfserveTimeEntryFormComponent,
    SelfserviceNewOvertimeComponent,
    SelfserveOfficialBusinessComponent,
    SelfserveTimeEntryComponent,
    PayperiodSelectComponent,
    TopbarComponent,
    SelfServiceDashboardComponent,
    SelfserviceOvertimesComponent,
    SelfServiceTimesheetsComponent,
  ],
  imports: [MatChipsModule, SharedModule, LeavesModule, TimeEntryModule],
})
export class SelfServiceModule {}
