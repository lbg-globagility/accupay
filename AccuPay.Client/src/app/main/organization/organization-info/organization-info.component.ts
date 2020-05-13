import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FileProgress } from 'src/app/files/shared/file-progress';
import { FormGroup } from '@angular/forms';
import { Organization } from 'src/app/accounts/shared/organization';
import { OrganizationFormService } from 'src/app/main/organization/organization-form.service';
import { MatDialog } from '@angular/material';
import { ImageResizerDialogComponent } from 'src/app/ui/image-resizer-dialog/image-resizer-dialog.component';
import { ImageCroppedEvent } from 'ngx-image-cropper';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-organization-info',
  templateUrl: './organization-info.component.html',
  styleUrls: ['./organization-info.component.scss']
})
export class OrganizationInfoComponent implements OnInit {
  @Input()
  organization: Organization;

  @Output()
  save: EventEmitter<File> = new EventEmitter<File>();

  imageUrl: any;

  image: File = null;

  hide = true;

  currentProgress: FileProgress<Organization> = new FileProgress(
    'Not Started',
    0
  );

  constructor(
    private formService: OrganizationFormService,
    private dialog: MatDialog,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit() {
    const moment = new Date().getTime().toString();
    this.formService.patchInfo(this.organization);
    this.imageUrl = `api/account/organization/image/${this.organization.id}?${moment}`;
  }

  get form(): FormGroup {
    return this.formService.form;
  }

  isInProgress() {
    return !(
      this.currentProgress.isFailed() || this.currentProgress.isSuccess()
    );
  }

  onSelect(file: FileList) {
    this.image = file.item(0);

    if (file.item(0) === null || file.item(0) === undefined) {
      this.imageUrl = 'assets/No.jpg';
    } else {
      this.openCropperDialog(this.image);
    }
  }

  private openCropperDialog(image: File): void {
    const dialog = this.dialog.open(ImageResizerDialogComponent, {
      width: '80vw',
      data: {
        image,
        maintainAspectRatio: false,
        roundCropper: false,
        aspectRatio: 1 / 1
      }
    });

    dialog.afterClosed().subscribe((event: ImageCroppedEvent) => {
      if (event == null) {
        return;
      }

      const croppedFile = new File([event.file], image.name, {
        type: image.type
      });

      this.image = croppedFile;
      this.readFile(this.image);
    });
  }

  private readFile(image: File): void {
    const reader = new FileReader();
    reader.onload = (event: any) => {
      this.imageUrl = this.sanitizer.bypassSecurityTrustResourceUrl(
        event.target.result
      );
    };

    reader.readAsDataURL(image);
  }

  onSave() {
    if (!this.form.valid) {
      return;
    }

    this.save.emit(this.image);
  }
}
