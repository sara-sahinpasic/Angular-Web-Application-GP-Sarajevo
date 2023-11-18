import { Component, OnInit } from '@angular/core';
import { tap } from 'rxjs';
import { NewsResponseDto } from 'src/app/models/news/newsDto';
import { Pagination } from 'src/app/models/pagination/pagination';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { ModalService } from 'src/app/services/modal/modal.service';
import { NewsService } from 'src/app/services/news/news.service';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.scss'],
})
export class NewsComponent implements OnInit {
  protected newsTable: NewsResponseDto[] = [];
  protected paginationModel: Pagination = {
    pageSize: 7,
    page: 1,
  };
  protected pageNumber: number = 0;
  protected pageNumbers: number[] = [];

  constructor(
    private modalService: ModalService,
    private newsService: NewsService,
    protected localizationService: LocalizationService
  ) {}


  ngOnInit() {
    this.getPageCount();
    this.loadNewsPage();
  }

  protected showNewsModalButton(id: string) {
    this.modalService.data = id;
    this.modalService.showNewsModal();
  }

  protected loadNewsPage() {
    this.newsService.getAllNews(this.paginationModel.page, this.paginationModel.pageSize)
      .pipe(
        tap((response: NewsResponseDto[]) => this.newsTable = response)
      )
      .subscribe();
  }

  protected nextPage() {
    if (this.paginationModel.page >= this.pageNumber) {
      return;
    }

    this.paginationModel.page++;
    this.loadNewsPage();
  }

  protected prevPage() {
    if (this.paginationModel.page <= 1) {
      return;
    }

    this.paginationModel.page--;
    this.loadNewsPage();
  }

  protected getPageCount() {
    this.newsService.getNewsPageCount(this.paginationModel.pageSize)
    .pipe(
      tap((response: number) => {
        this.pageNumber = response;
        this.pageNumbers = Array(this.pageNumber).fill(1)
          .map((x, i) => x + i);
      })
    )
    .subscribe();
  }

  protected changePage(page: number) {
    this.paginationModel.page = page;
    this.loadNewsPage();
  }
}
