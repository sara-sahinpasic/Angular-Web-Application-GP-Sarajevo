import {
  HttpClient,
  HttpEventType,
  HttpHeaders,
  HttpRequest,
} from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { DataResponse } from 'src/app/models/DataResponse';
import { RequestDto } from 'src/app/models/Request/RequestDto';
import { UserStatusDto } from 'src/app/models/User/UserStatusDto';
import { UserService } from 'src/app/services/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-request',
  templateUrl: './request.component.html',
  styleUrls: ['./request.component.scss'],
})
export class RequestComponent implements OnInit {
  @Input() showModalRequest: boolean = false;
  @Output() close: EventEmitter<void> = new EventEmitter<void>();

  private url: string = environment.apiUrl;

  requests: Array<UserStatusDto> = [];
  request_id?: string;

  requestData: RequestDto = {
    userId: 'db7696ff-37b0-4db5-99c6-e44ab10b593f',
    userStatusId: '',
    //documentLink: '',
  };

  working = false;
  uploadFile?: File | null;
  uploadFileLabel: string | undefined = 'Choose a file to upload';
  uploadProgress?: number;
  uploadUrl?: string;

  constructor(private router: Router, private httpClient: HttpClient, private userService: UserService) {}

  ngOnInit(): void {
    const token: string | null = localStorage.getItem('token');
    const headers: HttpHeaders = new HttpHeaders({
      Authorization: 'Bearer ' + token,
    });

    this.httpClient
      .get<DataResponse<UserStatusDto[]>>(`${this.url}Profile/Status`, {
        headers,
      })
      .subscribe((r: DataResponse<UserStatusDto[]>) => {
        this.requests = r.data;
        this.requestData.userStatusId = r.data[0].id;
      });
  }

  handleFileInput(files: FileList) {
    if (files.length > 0) {
      this.uploadFile = files.item(0);
      this.uploadFileLabel = this.uploadFile?.name;
    }
  }

  upload() {
    if (!this.uploadFile) {
      alert('Choose a file to upload first');
      return;
    }

    console.log(this.requestData);

    const formData = new FormData();
    formData.append(this.uploadFile.name, this.uploadFile);
    formData.append("requestData", JSON.stringify(this.requestData));

    const token: string | null = localStorage.getItem('token');
    const headers: HttpHeaders = new HttpHeaders({
      Authorization: 'Bearer ' + token,
    });

    const urlAdress = `${this.url}RequestContoller/UploadFile`;
    const uploadReq = new HttpRequest('POST', urlAdress, formData, {
      reportProgress: true,
      headers,
    });

    this.uploadUrl = '';
    this.uploadProgress = 0;
    this.working = true;

    this.httpClient
      .request(uploadReq)
      .subscribe(
        (event: any) => {
          if (event.type === HttpEventType.UploadProgress) {
            this.uploadProgress = Math.round(
              (100 * event.loaded) / event.total
            );
          } else if (event.type === HttpEventType.Response) {
            //this.uploadUrl = event.body.url;
          }
        },
        (error: any) => {
          console.error(error);
        }
      )
      .add(() => {
        this.working = false;
      });
  }

  sendRequestButton() {}
}
