import { formatDate } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { catchError, of, tap } from 'rxjs';
import { InvalidArgumentException } from 'src/app/exceptions/invalidArgumentException';
import { ApproveRequestModel } from 'src/app/models/request/approveRequestModel';
import { RejectRequestModel } from 'src/app/models/request/rejectRequestModel';
import { RequestModel } from 'src/app/models/request/requestModel';
import { SpecialRequestService } from 'src/app/services/special-request/special-request.service';

@Component({
  selector: 'app-admin-user-request-modal',
  templateUrl: './admin-user-request-modal.component.html',
  styleUrls: ['./admin-user-request-modal.component.scss']
})
export class AdminUserRequestModalComponent implements OnInit {
  @Input() public userId?: string;

  protected requestForm: FormGroup = {} as FormGroup;
  protected requestViewOnlyModel: RequestModel = {} as RequestModel;
  protected approvalModel: any = {
    approved: false,
    rejected: false,
    rejectionReason: '',
    dateOfExpiraton: null
  };
  protected isApprovalContainerDirty: boolean = false;

  constructor(private requestService: SpecialRequestService, private formBuilder: FormBuilder) {}

  ngOnInit() {
    if (!this.userId) {
      throw new InvalidArgumentException(['userId']);
    }

    setTimeout(() => {
      this.initializeForm();
      this.getRequestData();
    }, 0);
  }

  private getRequestData() {
    this.requestService.getRequestForUser(this.userId!)
      .pipe(
        tap(this.mapToViewOnlyModel.bind(this)),
        catchError(() => {
          throw new Error('No data present.');
        })
      )
      .subscribe();
  }

  private initializeForm() {
    this.requestForm = this.formBuilder.group({
      rejectionReason: ['', Validators.required],
      dateOfExpiration: [null, Validators.required]
    });
  }

  private mapToViewOnlyModel(requestModel: RequestModel) {
    this.requestViewOnlyModel = requestModel;
    this.requestViewOnlyModel.dateCreated = formatDate(requestModel.dateCreated, 'yyyy-MM-dd', 'en');
  }

  protected sendAnswer() {
    this.markAsDirty();

    if (!this.validateInputs()) {
      return;
    }

    if (this.approvalModel.approved) {
      this.approveRequest();
      return;
    }

    this.rejectRequest();
  }

  private validateInputs(): boolean {
    if (!this.approvalModel.approved && !this.approvalModel.rejected) {
      return false;
    }

    if (!this.approvalModel.rejectionReason && this.approvalModel.rejected) {
      return false;
    }

    if (!this.approvalModel.dateOfExpiration && this.approvalModel.approved) {
      return false;
    }

    return true;
  }

  private approveRequest() {
    const approveRequestModel: ApproveRequestModel = {
      expirationDate: this.approvalModel.dateOfExpiration
    };

    this.requestService.approveRequest(this.requestViewOnlyModel.requestId, approveRequestModel)
      .subscribe(this.reloadPage);
  }

  private rejectRequest() {
    const rejectRequestModal: RejectRequestModel = {
      rejectionReason: this.approvalModel.rejectionReason
    };

    this.requestService.rejectRequest(this.requestViewOnlyModel.requestId, rejectRequestModal)
      .subscribe(this.reloadPage);
  }

  private reloadPage() {
    setTimeout(() => {
      location.reload();
    }, 1500);
  }

  protected changeState(changedCheckbox: HTMLInputElement, checkboxToChange: HTMLInputElement) {
    if (changedCheckbox.checked) {
      checkboxToChange.checked = false;
    }

    this.approvalModel.approved = changedCheckbox.name == 'approve' ? changedCheckbox.checked : checkboxToChange.checked;
    this.approvalModel.rejected = changedCheckbox.name == 'reject' ? changedCheckbox.checked : checkboxToChange.checked;

    this.isApprovalContainerDirty = false;
  }

  protected markAsDirty() {
    this.isApprovalContainerDirty = true;
  }
}
