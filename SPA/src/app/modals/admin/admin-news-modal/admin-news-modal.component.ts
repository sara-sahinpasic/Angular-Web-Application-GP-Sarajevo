import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NewsDto } from 'src/app/models/Admin/News/NewsDto';
import { AdminNewsService } from 'src/app/services/admin/news/admin-news.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-admin-news-modal',
  templateUrl: './admin-news-modal.component.html',
  styleUrls: ['./admin-news-modal.component.scss'],
})
export class AdminNewsModalComponent implements OnInit {
  constructor(
    protected formBuilder: FormBuilder,
    protected newsService: AdminNewsService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.inizializeValidators();
  }
  protected registrationForm!: FormGroup;
  protected newsModel: NewsDto = {};

  publish() {
    this.registrationForm.markAllAsTouched();

    if (this.registrationForm.valid) {
      this.userService.user$.subscribe((user) => {
        this.newsModel.userId = user?.id;
      });

      this.newsService.publishNews(this.newsModel).subscribe(() => {
        setTimeout(function () {
          window.location.reload();
        }, 3000);
      });
    }
  }

  inizializeValidators() {
    this.registrationForm = this.formBuilder.group({
      title: ['', Validators.required],
      content: ['', Validators.required],
    });
  }
}
