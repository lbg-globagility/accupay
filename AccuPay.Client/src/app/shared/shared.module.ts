import { CommonModule } from '@angular/common';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from 'src/app/shared/material.module';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { IfEmptyPipe } from 'src/app/core/pipes/if-empty.pipe';
import { IfZeroPipe } from 'src/app/core/pipes/if-zero.pipe';
import { AmountPipe } from 'src/app/shared/pipes/amount.pipe';
import { ImgFallbackDirective } from 'src/app/shared/components/imgfallback/imgfallback.directive';
import { StatusComponent } from 'src/app/shared/components/status/status.component';
import { NgxPermissionsModule } from 'ngx-permissions';
import { DisplayFieldComponent } from 'src/app/shared/components/display-field/display-field.component';
import { ExpandableRowComponent } from 'src/app/shared/components/expandable-row/expandable-row.component';
import { EmployeeAvatarComponent } from 'src/app/shared/components/employee-avatar/employee-avatar.component';
import { ExpandableRowContainerDirective } from 'src/app/shared/components/expandable-row/expandable-row-container.directive';
import { ExpandableRowTriggerDirective } from 'src/app/shared/components/expandable-row/expandable-row-trigger.directive';
import { YesNoPipe } from '../core/pipes/yes-no.pipe';
import { ButtonSpinnerComponent } from 'src/app/shared/components/button-spinner/button-spinner.component';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { PostImportParserOutputDialogComponent } from './import/post-import-parser-output-dialog/post-import-parser-output-dialog.component';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';

@NgModule({
  declarations: [
    StatusComponent,
    // LocalePipe,
    // SanitizerPipe,
    // BytesPipe,
    // CoalescePipe,
    EmployeeAvatarComponent,
    IfEmptyPipe,
    IfZeroPipe,
    AmountPipe,
    YesNoPipe,
    ButtonSpinnerComponent,
    DisplayFieldComponent,
    ExpandableRowComponent,
    ExpandableRowContainerDirective,
    ExpandableRowTriggerDirective,
    // ToggleButtonGroupComponent,
    // ToggleButtonComponent,
    // UserMiniInfoComponent,
    // ImageResizerDialogComponent,
    // PreviewDialogComponent,
    ImgFallbackDirective,
    PostImportParserOutputDialogComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    // MatDialogModule,
    MatIconModule,
    // MatTooltipModule,
    // MatDividerModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
    // MatProgressBarModule,
    MatTableModule,
    MatTabsModule,
  ],
  exports: [
    StatusComponent,
    EmployeeAvatarComponent,
    // LocalePipe,
    // SanitizerPipe,
    // BytesPipe,
    // CoalescePipe,
    IfEmptyPipe,
    IfZeroPipe,
    AmountPipe,
    YesNoPipe,
    ButtonSpinnerComponent,
    ImgFallbackDirective,
    DisplayFieldComponent,
    ExpandableRowComponent,
    ExpandableRowContainerDirective,
    ExpandableRowTriggerDirective,
    // UserMiniInfoComponent,
    NgxPermissionsModule,
    CommonModule,
    FormsModule,
    RouterModule,
    ReactiveFormsModule,
    DragDropModule,
    MaterialModule,
  ],
  entryComponents: [PostImportParserOutputDialogComponent],
})
export class SharedModule {}
