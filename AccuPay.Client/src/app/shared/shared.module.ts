import { CommonModule } from '@angular/common';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from 'src/app/shared/material.module';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { IfEmptyPipe } from 'src/app/core/pipes/if-empty.pipe';
import { IfZeroPipe } from 'src/app/core/pipes/if-zero.pipe';
import { ImgFallbackDirective } from 'src/app/shared/components/imgfallback/imgfallback.directive';
import { StatusComponent } from 'src/app/shared/components/status/status.component';
import { NgxPermissionsModule } from 'ngx-permissions';
import { DisplayFieldComponent } from 'src/app/shared/components/display-field/display-field.component';
import { AmountPipe } from 'src/app/shared/pipes/amount.pipe';
import { ExpandableRowComponent } from 'src/app/shared/components/expandable-row/expandable-row.component';
import { ExpandableRowContainerDirective } from 'src/app/shared/components/expandable-row/expandable-row-container.directive';
import { ExpandableRowTriggerDirective } from 'src/app/shared/components/expandable-row/expandable-row-trigger.directive';

@NgModule({
  declarations: [
    StatusComponent,
    // LocalePipe,
    // SanitizerPipe,
    // BytesPipe,
    // CoalescePipe,
    IfEmptyPipe,
    IfZeroPipe,
    AmountPipe,
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
  ],
  imports: [
    CommonModule,
    // MatDialogModule,
    // MatIconModule,
    // MatTooltipModule,
    // MatDividerModule,
    // MatButtonModule,
    // MatProgressBarModule,
  ],
  exports: [
    // LocalePipe,
    // SanitizerPipe,
    // BytesPipe,
    // CoalescePipe,
    IfEmptyPipe,
    IfZeroPipe,
    AmountPipe,
    ImgFallbackDirective,
    StatusComponent,
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
})
export class SharedModule {}
