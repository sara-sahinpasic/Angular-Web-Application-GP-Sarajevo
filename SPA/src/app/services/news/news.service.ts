import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NewsDto } from 'src/app/models/News/NewsDto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class NewsService {
  constructor(private httpClient: HttpClient) {}
  private apiUrl: string = environment.apiUrl;
  newsTable: Array<NewsDto> = [];

  getNewsById(id: string) {
    return this.httpClient.get(`${this.apiUrl}News/GetNewsById?id=${id}`);
  }
}
