import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NewsRequestDto } from 'src/app/models/News/NewsDto';
import { ModalService } from 'src/app/services/modal/modal.service';
import { NewsService } from 'src/app/services/news/news.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-admin-news-modal',
  templateUrl: './admin-news-modal.component.html',
  styleUrls: ['./admin-news-modal.component.scss'],
})
export class AdminNewsModalComponent implements OnInit {

  @Input() news?: NewsRequestDto;
  protected registrationForm!: FormGroup;
  protected newsModel: NewsRequestDto = {};

  constructor(
    protected formBuilder: FormBuilder,
    protected newsService: NewsService,
    private userService: UserService,
    private modalService: ModalService
  ) {}

  ngOnInit(): void {
    this.inizializeValidators();
    if (this.news) {
      this.newsModel = JSON.parse(JSON.stringify(this.news)); // deep copying object to prevent real time editing
    }
  }

  publish() {
    this.registrationForm.markAllAsTouched();

    if (this.registrationForm.valid) {
      this.userService.user$.subscribe((user) => {
        this.newsModel.userId = user?.id;
      });

      this.newsService.publishNews(this.newsModel).subscribe(this.modalService.closeModal.bind(this.modalService));
    }
  }

  edit() {
    this.registrationForm.markAllAsTouched();

    if (this.registrationForm.invalid) {
      return;
    }

    this.newsService.updateNews(this.newsModel)
      .subscribe(this.reloadPage);
  }

 private reloadPage() {
    setTimeout(() => {
      location.reload();
    }, 1500);
  }

  inizializeValidators() {
    this.registrationForm = this.formBuilder.group({
      title: ['', Validators.required],
      content: ['', Validators.required],
    });
  }
}
