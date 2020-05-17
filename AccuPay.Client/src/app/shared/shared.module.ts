import { CommonModule } from '@angular/common';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from 'src/app/shared/material.module';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    // StatusComponent,
    // LocalePipe,
    // SanitizerPipe,
    // BytesPipe,
    // CoalescePipe,
    // IfEmptyPipe,
    // ToggleButtonGroupComponent,
    // ToggleButtonComponent,
    // UserMiniInfoComponent,
    // ImageResizerDialogComponent,
    // PreviewDialogComponent,
    // ImgFallbackDirective,
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
    // IfEmptyPipe,
    // ImgFallbackDirective,
    // StatusComponent,
    // UserMiniInfoComponent,
    CommonModule,
    FormsModule,
    RouterModule,
    ReactiveFormsModule,
    DragDropModule,
    MaterialModule,
  ],
})
export class SharedModule {}
