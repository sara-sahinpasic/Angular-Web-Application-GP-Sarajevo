import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NewsResponseDto } from 'src/app/models/News/NewsDto';
import { NewsService } from 'src/app/services/news/news.service';

@Component({
  selector: 'app-news-modal',
  templateUrl: './news-modal.component.html',
  styleUrls: ['./news-modal.component.scss'],
})
export class NewsModalComponent implements OnInit {
  constructor(
    public route: ActivatedRoute,
    private newsService: NewsService
  ) {}

  @Input() newsId?: string;

  newsModel: NewsResponseDto = {
    title: '',
    content: '',
    id: '',
  };

  ngOnInit(): void {
    this.newsService.getNewsById(this.newsId!).subscribe((n: any) => {
      this.newsModel = n.data;
    });
  }
}
