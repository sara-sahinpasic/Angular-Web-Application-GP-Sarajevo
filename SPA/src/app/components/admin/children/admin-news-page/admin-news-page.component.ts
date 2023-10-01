import { Component, OnInit } from '@angular/core';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { tap } from 'rxjs';
import { InvalidArgumentException } from 'src/app/exceptions/InvalidArgumentException';
import { NewsResponseDto } from 'src/app/models/News/NewsDto';
import { Pagination } from 'src/app/models/Pagination/Pagination';
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
    const includeCreatedBy: boolean = true;

    this.newsService.getAllNews(includeCreatedBy)
      .pipe(
        tap(this.loadNews.bind(this))
      )
      .subscribe();
  }

  loadNews(data: NewsResponseDto[]) {
    this.newsList = data;
    this.filteredNewsList = data;
  }

  onPageChange(event) {
    this.paginationModel.page = event;
  }

  showNewsEditModal(news: NewsResponseDto) {
    this.modalService.data = news;
    this.modalService.adminShowNewsModal(news.title);
  }

  deleteNews(news: NewsResponseDto) {
    if (!news.id) {
      throw new InvalidArgumentException(['news'])
    }

    this.newsService.deleteNews(news.id)
      .subscribe(() => {
        this.removeNewsItem(news);
      });
  }

  reloadPage() {
    setTimeout(() => {
      location.reload();
    }, 3000);
  }

  removeNewsItem(news: NewsResponseDto) {
    this.newsList = this.newsList.filter(n => n.id?.toLowerCase() !== news.id?.toLowerCase());
    this.filteredNewsList = this.filteredNewsList.filter(n => n.id?.toLowerCase() !== news.id?.toLowerCase());

    if (this.filteredNewsList.length % this.paginationModel.pageSize! === 0
      && this.filteredNewsList.length > this.paginationModel.pageSize!) {
      --this.paginationModel.page!;
    }
  }

  findByNewsTitle(event: KeyboardEvent) {
    const target: HTMLInputElement = event.target as HTMLInputElement;

    if (!target.value) {
      this.resetFilter();
      return;
    }

    this.filteredNewsList = this.newsList.filter(news => news.title!.toLowerCase().indexOf(target.value.toLowerCase()) > -1);
  }

  resetFilter() {
    this.filteredNewsList = this.newsList;
  }
}
