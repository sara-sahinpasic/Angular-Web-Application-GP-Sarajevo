import { HttpEvent, HttpEventType } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { finalize, tap } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { RequestDto } from 'src/app/models/Request/RequestDto';
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';
import { UserStatusDto } from 'src/app/models/User/UserStatusDto';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { SpecialRequestService } from 'src/app/services/special-request/special-request.service';
import { UserStatusService } from 'src/app/services/user/user-status.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-request',
  templateUrl: './request.component.html',
  styleUrls: ['./request.component.scss']
})
export class RequestComponent implements OnInit {

  protected userStatuses: Array<UserStatusDto> = [];
  protected working = false;
  protected uploadFile?: File | null;
  protected uploadProgress?: number;
  protected requestData: RequestDto = {
    userId: '',
    userStatusId: '',
  };
  private allowedFileTypes: string[] = [
    'image/jpeg',
    'image/jpg',
    'image/png',
    'application/pdf',
  ];

  constructor(
    private userStatusService: UserStatusService,
    private specialRequestService: SpecialRequestService,
    private userService: UserService,
    protected localizationService: LocalizationService
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
    }
  }

  sendRequestButton() {
    if (!this.uploadFile) {
      alert(this.localizationService.localize("request_modal_no_file_selected_error_message"));
      return;
    }

    if (this.allowedFileTypes.indexOf(this.uploadFile.type) == -1) {
      alert(this.localizationService.localize("request_modal_file_not_supported_error_message"));
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
