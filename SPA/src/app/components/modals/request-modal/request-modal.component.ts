import { HttpEvent, HttpEventType } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { finalize, tap } from 'rxjs';
import { DataResponse } from 'src/app/models/dataResponse';
import { RequestDto } from 'src/app/models/request/requestDto';
import { UserProfileModel } from 'src/app/models/user/userProfileModel';
import { UserStatusDto } from 'src/app/models/user/userStatusDto';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { ModalService } from 'src/app/services/modal/modal.service';
import { SpecialRequestService } from 'src/app/services/special-request/special-request.service';
import { UserStatusService } from 'src/app/services/user/user-status.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-request-modal',
  templateUrl: './request-modal.component.html',
  styleUrls: ['./request-modal.component.scss']
})
export class RequestModalComponent implements OnInit {

  protected userStatuses: UserStatusDto[] = [];
  protected working = false;
  protected uploadFile?: File | null;
  protected uploadProgress: number = 0;
  protected requestData: RequestDto = {} as RequestDto;
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
    private modalService: ModalService,
    protected localizationService: LocalizationService,
  ) {}


  ngOnInit() {
    // hacky way to fix the ExpressionChangedAfterItHasBeenCheckedError bug
    // with this async call we call the Angular change detection again (there was something about the Macro and Micro stack but I haven't delved much into it)
    setTimeout(() => {
      this.setup();
    }, 0);
  }

  private setup() {
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

  protected handleFileInput(files: FileList) {
    if (files.length > 0) {
      this.uploadFile = files.item(0);
    }
  }

  protected sendRequestButton() {
    if (!this.uploadFile) {
      alert(this.localizationService.localize('request_modal_no_file_selected_error_message'));
      return;
    }

    if (this.allowedFileTypes.indexOf(this.uploadFile.type) == -1) {
      alert(this.localizationService.localize('request_modal_file_not_supported_error_message'));
      return;
    }

    this.requestData.document = this.uploadFile;
    this.uploadProgress = 0;
    this.working = true;

    this.specialRequestService
      .requestDiscount(this.requestData)
      .pipe(
        tap(this.showProgress.bind(this)),
        finalize(() => {
          this.working = false;
          this.modalService.closeModal();
        })
      )
      .subscribe();
  }

  private showProgress(event: HttpEvent<DataResponse<string>>): any {
    if (event.type === HttpEventType.UploadProgress) {
      this.uploadProgress = Math.round((100 * event.loaded) / event.total!);
    }
  }
}
