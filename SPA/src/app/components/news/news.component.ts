import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DataResponse } from 'src/app/models/DataResponse';
import { NewsDto } from 'src/app/models/News/NewsDto';
import { Pagination } from 'src/app/models/Pagination/Pagination';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { ModalService } from 'src/app/services/modal/modal.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.scss'],
})
export class NewsComponent implements OnInit {

  constructor(
    private modalService: ModalService,
    private httpClient: HttpClient,
    protected localizationService: LocalizationService
  ) {}

  newsTable: Array<NewsDto> = [];

  public paginationModel: Pagination = {
    pageSize: 7,
    page: 1,
  };

  pages: number = 0;
  pagesArray: Array<number> = [];

  private apiUrl: string = environment.apiUrl;

  ngOnInit(): void {
    this.getPageSize();
    this.getPage();
  }

  showNewsModalButton(n: string) {
    console.log(n);
    this.modalService.data = n;
    this.modalService.showNewsModal();
  }

  getPage() {
    this.httpClient
      .get(
        `${this.apiUrl}News/Pagination?page=${this.paginationModel.page}&pageSize=${this.paginationModel.pageSize}`
      )
      .subscribe((x: any) => {
        this.newsTable = x.data;
      });
  }

  nextPage() {
    if ((this.paginationModel.page as number) >= this.pages) {
      return;
    }

    (this.paginationModel.page as number)++;
    this.getPage();
  }

  prevPage() {
    if ((this.paginationModel.page as number) <= 1) {
      return;
    }
    (this.paginationModel.page as number)--;
    this.getPage();
  }

  getPageSize() {
    this.httpClient
      .get<DataResponse<number>>(
        `${this.apiUrl}News/NewsPagesCount?pageSize=${this.paginationModel.pageSize}`
      )
      .subscribe((response: DataResponse<number>) => {
        this.pages = response.data;
        this.pagesArray = Array(this.pages)
          .fill(0)
          .map((x, i) => i + 1);
      });
  }
  changePage(page: number) {
    this.paginationModel.page = page;
    this.getPage();
  }
}
