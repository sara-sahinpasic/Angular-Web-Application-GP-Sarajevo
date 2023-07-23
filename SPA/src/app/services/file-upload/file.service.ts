import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Observable, map, tap } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { UserStatusDto } from 'src/app/models/User/UserStatusDto';
import { environment } from 'src/environments/environment';
import { UserService } from '../user/user.service';
import { Buffer } from 'buffer';
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';


@Injectable({
  providedIn: 'root',
})
export class FileService {

  private baseApiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient, private userService: UserService) {}

  download(url: string): Observable<HttpResponse<Blob>> {
    const token: string | null = this.userService.getUserJwtToken();
    let userId: string | undefined= "";
    this.userService.user$.pipe(
      tap((user: UserProfileModel | undefined) => userId = user?.id)
    )
    .subscribe();

    const headers: HttpHeaders = new HttpHeaders({
      Authorization: 'Bearer ' + token,
    });

    return this.httpClient.get(`${this.baseApiUrl}${url}?userId=${userId}`, { headers, responseType: 'blob', observe: 'response' })
      .pipe(
        tap((response: HttpResponse<Blob>) => {
          const fileContents: Blob = response.body as Blob;
          console.log(response)
          const fileName: string = this.getFileNameFromResponseHeader(response);

          this.saveFile(fileName, fileContents);
        })
      );
  }

  private saveFile(fileName: string, fileContents: Blob) {
    const link = document.createElement('a');
    link.href = window.URL.createObjectURL(fileContents);
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  private getFileNameFromResponseHeader(response: HttpResponse<Blob>): string {
    return response.headers.get("Content-Disposition")?.split("; ")[1].split("=")[1] as string;
  }
}
