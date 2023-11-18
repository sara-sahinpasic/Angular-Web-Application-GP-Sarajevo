import { Component, OnInit } from '@angular/core';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { tap } from 'rxjs';
import { InvalidArgumentException } from 'src/app/exceptions/invalidArgumentException';
import { NewsResponseDto } from 'src/app/models/news/newsDto';
import { Pagination } from 'src/app/models/pagination/pagination';
import { ModalService } from 'src/app/services/modal/modal.service';
import { NewsService } from 'src/app/services/news/news.service';

@Component({
  selector: 'app-admin-news-page',
  templateUrl: './admin-news-page.component.html',
  styleUrls: ['./admin-news-page.component.scss']
})
export class AdminNewsPageComponent implements OnInit {
  protected paginationModel: Pagination = {
    pageSize: 5,
    page: 1,
    totalCount: 0,
  };
  protected newsList: NewsResponseDto[] = [];
  protected filteredNewsList: NewsResponseDto[] = [];
  protected editIcon = faEdit;
  protected deleteIcon = faTrash;

  constructor(private newsService: NewsService, private modalService: ModalService) {}

  ngOnInit() {
    this.newsService.getAllNews()
      .pipe(
        tap(this.loadNews.bind(this))
      )
      .subscribe();
  }

  private loadNews(data: NewsResponseDto[]) {
    this.newsList = data;
    this.filteredNewsList = data;
  }

  protected onPageChange(event) {
    this.paginationModel.page = event;
  }

  protected showNewsEditModal(news: NewsResponseDto) {
    this.modalService.data = news;
    this.modalService.adminShowNewsModal(news.title);
  }

  protected deleteNews(news: NewsResponseDto) {
    if (!news.id) {
      throw new InvalidArgumentException(['news'])
    }

    this.newsService.deleteNews(news.id)
      .subscribe(this.reloadPage);
  }

  private reloadPage() {
    setTimeout(() => {
      location.reload();
    }, 1500);
  }

  protected findByNewsTitle(event: KeyboardEvent) {
    const target: HTMLInputElement = event.target as HTMLInputElement;

    if (!target.value) {
      this.resetFilter();
      return;
    }

    this.filteredNewsList = this.newsList.filter(news => news.title!.toLowerCase().indexOf(target.value.toLowerCase()) > -1);
  }

  private resetFilter() {
    this.filteredNewsList = this.newsList;
  }
}
