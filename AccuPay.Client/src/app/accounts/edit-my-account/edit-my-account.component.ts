import Swal from 'sweetalert2';
import { Account } from '../shared/account';
import { AccountService } from '../services/account.service';
import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { FileProgress } from 'src/app/files/shared/file-progress';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ImageCroppedEvent } from 'ngx-image-cropper';
import { ImageResizerDialogComponent } from 'src/app/ui/image-resizer-dialog/image-resizer-dialog.component';
import { MatDialog } from '@angular/material';
import { PhonePipe } from 'src/app/users/shared/phone-pipe';
import { Router } from '@angular/router';
import { TopbarAccountService } from 'src/app/main/service/topbar-account.service';

const US_ISO_CODE = 'US';

@Component({
  selector: 'app-edit-my-account',
  templateUrl: './edit-my-account.component.html',
  styleUrls: ['./edit-my-account.component.scss'],
  providers: [PhonePipe]
})
export class EditMyAccountComponent implements OnInit {
  imageUrl: any;

  image: File = null;

  detail: Account;

  currentProgress: FileProgress<Account> = new FileProgress('Not Started', 0);

  form: FormGroup = this.fb.group({
    id: [null],
    email: [null, Validators.required],
    title: [null],
    supplierId: [null],
    providerId: [null],
    companyId: [null],
    firstName: [null, Validators.required],
    lastName: [null, Validators.required],
    mobilePhone: [null],
    phoneNumber: [null],
    location: [null],
    image: [null]
  });

  moment = new Date().getTime().toString();

  isAdminRole: boolean = false;

  constructor(
    private accountService: AccountService,
    private topbarAccountService: TopbarAccountService,
    private dialog: MatDialog,
    private fb: FormBuilder,
    private router: Router,
    private sanitizer: DomSanitizer,
    private phonePipe: PhonePipe
  ) {}

  ngOnInit(): void {
    this.accountService.get().subscribe(data => {
      if (data != null) {
        this.detail = data;
        this.form.patchValue(this.detail);
        this.imageUrl = `/api/account/image/${this.detail.id}?${this.moment}`;
        this.isAdminRole = data.role2.isAdmin;
      }
    });
  }

  isInProgress(): boolean {
    return !(
      this.currentProgress.isFailed() || this.currentProgress.isSuccess()
    );
  }

  onSelect(files: FileList): void {
    const file = files.item(0);

    if (file == null) {
      this.imageUrl = 'assets/No.jpg';
    } else {
      this.openCropperDialog(file);
    }
  }

  private openCropperDialog(image: File): void {
    const dialog = this.dialog.open(ImageResizerDialogComponent, {
      width: '80vw',
      data: { image, roundCropper: true }
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

  save(): void {
    const id = this.detail.id;
    const upload$ = this.accountService.upload(this.image, id);

    this.userPhoneNumber();

    upload$.subscribe({
      next: progress => {
        if (progress.isSuccess()) {
          const { body: account } = progress;

          this.form.get('id').setValue(id);
          this.form.get('image').setValue(account.image);
        }

        this.currentProgress = progress;
      },
      complete: () => {
        if (this.currentProgress.isSuccess()) {
          this.accountService.update(this.form.value).subscribe(() => {
            this.topbarAccountService.setData();
            setTimeout(() => {
              this.showSuccessDialog();
              this.router.navigate(['myaccount']);
            });
          });
        }
      }
    });
  }

  cancel(): void {
    this.router.navigate(['myaccount']);
  }

  private showSuccessDialog(): void {
    Swal.fire({
      title: 'Complete',
      text: 'Successfully saved!',
      icon: 'success',
      timer: 3000,
      showConfirmButton: false
    });
  }

  private userPhoneNumber() {
    const phoneNumberValue = this.phoneNumberValue;
    if (this.phoneNumberControl.value == null) {
      return;
    }

    const pipedPhoneNumber = this.phonePipe.transform(
      phoneNumberValue,
      US_ISO_CODE
    );
    this.phoneNumberControl.setValue(pipedPhoneNumber);
  }

  get phoneNumberControl() {
    return this.form.get('phoneNumber');
  }

  get phoneNumberValue() {
    return this.phoneNumberControl.value;
  }
}
