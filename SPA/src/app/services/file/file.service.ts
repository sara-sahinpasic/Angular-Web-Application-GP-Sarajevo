import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserService } from '../user/user.service';
import { UserProfileModel } from 'src/app/models/User/UserProfileModel';


@Injectable({
  providedIn: 'root',
})
export class FileService {

  private baseApiUrl = environment.apiUrl;
  private userId?: string;

  constructor(private httpClient: HttpClient, private userService: UserService) {
    this.userService.user$.pipe(
      tap((user: UserProfileModel | undefined) => this.userId = user?.id)
    )
    .subscribe();
  }

  download(url: string): Observable<HttpResponse<Blob>> {
    return this.httpClient.get(`${this.baseApiUrl}${url}?userId=${this.userId}`, { responseType: 'blob', observe: 'response' })
      .pipe(
        tap((response: HttpResponse<Blob>) => {
          const fileContents: Blob = response.body as Blob;
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
