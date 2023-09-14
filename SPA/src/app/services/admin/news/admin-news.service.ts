import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { NewsDto } from 'src/app/models/Admin/News/NewsDto';
import { DataResponse } from 'src/app/models/DataResponse';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AdminNewsService {
  private apiUrl: string = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  public publishNews(newsModel: NewsDto): Observable<DataResponse<NewsDto[]>> {
    return this.httpClient.post<DataResponse<NewsDto[]>>(
      `${this.apiUrl}News`,
      newsModel
    );
  }
}
