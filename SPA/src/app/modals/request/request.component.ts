import { HttpEvent, HttpEventType } from '@angular/common/http';
import { Component } from '@angular/core';
import { finalize, tap } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { RequestDto } from 'src/app/models/Request/RequestDto';
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';
import { UserStatusDto } from 'src/app/models/User/UserStatusDto';
import { SpecialRequestService } from 'src/app/services/special-request/special-request.service';
import { UserStatusService } from 'src/app/services/user/user-status.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-request',
  templateUrl: './request.component.html',
  styleUrls: ['./request.component.scss']
})
export class RequestComponent {
  private allowedFileTypes: string[] = [
    'image/jpeg',
    'image/jpg',
    'image/png',
    'application/pdf',
  ];
  protected userStatuses: Array<UserStatusDto> = [];
  protected requestData: RequestDto = {
    userId: '',
    userStatusId: '',
  };
  protected working = false;
  protected uploadFile?: File | null;
  protected uploadFileLabel: string | undefined = 'Choose a file to upload';
  protected uploadProgress?: number;

  constructor(
    private userStatusService: UserStatusService,
    private specialRequestService: SpecialRequestService,
    private userService: UserService
  ) {}


  ngOnInit(): void {
    // hacky way to fix the ExpressionChangedAfterItHasBeenCheckedError bug
    // with this async call we call the Angular change detection again (there was something about the Macro and Micro stack but I haven't delved much into it)
    setTimeout(() => {
      this.setup();
    }, 0);
  }

  setup(): void {
    this.userStatusService
      .getAvailableUserStatuses()
      .pipe(
        tap((response: DataResponse<UserStatusDto[]>) => {
          this.userStatuses = response.data;
          this.requestData.userStatusId = response.data[0].id;
        })
      )
      .subscribe();

    this.userService.user$
      .pipe(
        tap(
          (user: UserProfileModel | undefined) =>
            (this.requestData.userId = user?.id)
        )
      )
      .subscribe();
  }

  handleFileInput(files: FileList) {
    if (files.length > 0) {
      this.uploadFile = files.item(0);
      this.uploadFileLabel = this.uploadFile?.name;
    }
  }

  sendRequestButton() {
    if (!this.uploadFile) {
      alert('Choose a file to upload first');
      return;
    }

    if (this.allowedFileTypes.indexOf(this.uploadFile.type) == -1) {
      alert('This file type is not supported');
      return;
    }

    this.requestData.document = this.uploadFile;
    this.uploadProgress = 0;
    this.working = true;

    this.specialRequestService
      .requestDiscount(this.requestData)
      .pipe(
        tap(this.showProgress.bind(this)),
        finalize(() => (this.working = false))
      )
      .subscribe();
  }

  private showProgress(event: HttpEvent<DataResponse<string>>): any {
    if (event.type === HttpEventType.UploadProgress) {
      this.uploadProgress = Math.round((100 * event.loaded) / event.total!);
    }
  }
}
