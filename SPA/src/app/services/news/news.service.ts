import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { NewsRequestDto, NewsResponseDto } from 'src/app/models/News/NewsDto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class NewsService {
  private apiUrl: string = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  getNewsById(id: string) {
    return this.httpClient.get(`${this.apiUrl}News/GetNewsById?id=${id}`);
  }

  getAllNews(includeCreatedBy: boolean = false): Observable<NewsResponseDto[]> {
    return this.httpClient.get<DataResponse<NewsResponseDto[]>>(`${this.apiUrl}News/All?includeCreatedBy=${includeCreatedBy}`)
      .pipe(
        map((response: DataResponse<NewsResponseDto[]>) => response.data)
      );
  }

  publishNews(newsModel: NewsRequestDto): Observable<unknown> {
    return this.httpClient.post(`${this.apiUrl}News`, newsModel);
  }

  updateNews(news: NewsRequestDto): Observable<unknown> {
    return this.httpClient.put(`${this.apiUrl}Admin/News/Edit/${news.id}`, news);
  }

  deleteNews(newsId: string): Observable<unknown> {
    return this.httpClient.delete(`${this.apiUrl}Admin/News/Delete/${newsId}`);
  }
}
