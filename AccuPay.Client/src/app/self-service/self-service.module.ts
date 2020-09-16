import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { SelfServeComponent } from './self-serve/self-serve.component';
import { SelfserveLeaveFormComponent } from './components/selfserve-leave-form/selfserve-leave-form.component';
import { SelfserveOvertimeFormComponent } from './components/selfserve-overtime-form/selfserve-overtime-form.component';
import { SelfserveOfficialBusinessFormComponent } from './components/selfserve-official-business-form/selfserve-official-business-form.component';
import { SelfserveTimeEntryFormComponent } from './components/selfserve-time-entry-form/selfserve-time-entry-form.component';
import { LeavesModule } from '../leaves/leaves.module';
import { SelfserveLeaveComponent } from './pages/selfserve-leave/selfserve-leave.component';
import { SelfserveOvertimeComponent } from './pages/selfserve-overtime/selfserve-overtime.component';
import { SelfserveOfficialBusinessComponent } from './pages/selfserve-official-business/selfserve-official-business.component';
import { SelfserveTimeEntryComponent } from './pages/selfserve-time-entry/selfserve-time-entry.component';
import { TimeEntryModule } from '../time-entry/time-entry.module';
import { MatChipsModule } from '@angular/material/chips';
import { PayperiodSelectComponent } from './components/payperiod-select/payperiod-select.component';
import { TopbarComponent } from './components/topbar/topbar.component';
import { SelfServiceDashboardComponent } from './components/self-service-dashboard/self-service-dashboard.component';
import { SelfServiceLeavesComponent } from './components/self-service-leaves/self-service-leaves.component';
import { SelfServiceTimesheetsComponent } from './components/self-service-timesheets/self-service-timesheets.component';
import { SelfServiceOvertimesComponent } from 'src/app/self-service/overtimes/self-service-overtimes/self-service-overtimes.component';

@NgModule({
  declarations: [
    SelfServeComponent,
    SelfserveLeaveFormComponent,
    SelfserveOvertimeFormComponent,
    SelfserveOfficialBusinessFormComponent,
    SelfserveTimeEntryFormComponent,
    SelfserveLeaveComponent,
    SelfserveOvertimeComponent,
    SelfserveOfficialBusinessComponent,
    SelfserveTimeEntryComponent,
    PayperiodSelectComponent,
    TopbarComponent,
    SelfServiceDashboardComponent,
    SelfServiceLeavesComponent,
    SelfServiceOvertimesComponent,
    SelfServiceTimesheetsComponent,
  ],
  imports: [MatChipsModule, SharedModule, LeavesModule, TimeEntryModule],
})
export class SelfServiceModule {}
