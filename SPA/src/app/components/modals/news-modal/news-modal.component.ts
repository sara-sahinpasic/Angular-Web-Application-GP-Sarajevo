import { Component, Input, OnInit } from '@angular/core';
import { NewsResponseDto } from 'src/app/models/news/newsDto';
import { NewsService } from 'src/app/services/news/news.service';

@Component({
  selector: 'app-news-modal',
  templateUrl: './news-modal.component.html',
  styleUrls: ['./news-modal.component.scss'],
})
export class NewsModalComponent implements OnInit {
  @Input() newsId?: string;
  protected newsModel: NewsResponseDto = {} as NewsResponseDto;

  constructor(private newsService: NewsService) {}

  ngOnInit() {
    setTimeout(() => {
      this.newsService.getNewsById(this.newsId!).subscribe((n: any) => {
        this.newsModel = n.data;
      });
    }, 0);
  }
}
