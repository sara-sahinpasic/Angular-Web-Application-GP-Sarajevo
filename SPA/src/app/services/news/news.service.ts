import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { DataResponse } from 'src/app/models/dataResponse';
import { NewsRequestDto, NewsResponseDto } from 'src/app/models/news/newsDto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class NewsService {
  private apiUrl: string = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  public getNewsById(id: string) {
    return this.httpClient.get(`${this.apiUrl}News/Get/${id}`);
  }

  public getNewsPageCount(pageSize: number): Observable<number> {
    return this.httpClient.get<DataResponse<number>>(
      `${this.apiUrl}News/PagesCount?pageSize=${pageSize}`
    )
    .pipe(
      map((response: DataResponse<number>) => response.data)
    );
  }

  public getAllNews(page: number | null = null, pageSize: number | null = null): Observable<NewsResponseDto[]> {
    let queryString: string = '';

    if (page && pageSize) {
      queryString = `?page=${page}&pageSize=${pageSize}`;
    }

    return this.httpClient.get<DataResponse<NewsResponseDto[]>>(`${this.apiUrl}News/All${queryString}`)
      .pipe(
        map((response: DataResponse<NewsResponseDto[]>) => response.data)
      );
  }

  public publishNews(newsModel: NewsRequestDto): Observable<unknown> {
    return this.httpClient.post(`${this.apiUrl}Admin/News/Publish`, newsModel);
  }

  public updateNews(news: NewsRequestDto): Observable<unknown> {
    return this.httpClient.put(`${this.apiUrl}Admin/News/Edit/${news.id}`, news);
  }

  public deleteNews(newsId: string): Observable<unknown> {
    return this.httpClient.delete(`${this.apiUrl}Admin/News/Delete/${newsId}`);
  }
}
