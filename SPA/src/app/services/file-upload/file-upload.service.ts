import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { UserStatusDto } from 'src/app/models/User/UserStatusDto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class FileUploadService implements OnInit {
  baseApiUrl = 'https://file.io';
  constructor(private httpClient: HttpClient) {}

  upload(file:any): Observable<any> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    return this.httpClient.post(this.baseApiUrl, formData);
  }

  ngOnInit(): void {}
}
