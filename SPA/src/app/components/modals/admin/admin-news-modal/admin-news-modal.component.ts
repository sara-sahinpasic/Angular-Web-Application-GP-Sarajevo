import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NewsRequestDto } from 'src/app/models/news/newsDto';
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
  protected registrationForm: FormGroup = {} as FormGroup;
  protected newsModel: NewsRequestDto = {} as NewsRequestDto;

  constructor(
    private formBuilder: FormBuilder,
    private newsService: NewsService,
    private userService: UserService,
    private modalService: ModalService
  ) {}

  ngOnInit() {
    this.initializeForm();
    if (this.news) {
      this.newsModel = JSON.parse(JSON.stringify(this.news)); // deep copying object to prevent real time editing
    }
  }

  protected publish() {
    this.registrationForm.markAllAsTouched();

    if (this.registrationForm.valid) {
      this.userService.user$.subscribe((user) => {
        this.newsModel.userId = user?.id!;
      });

      this.newsService.publishNews(this.newsModel).subscribe(this.modalService.closeModal.bind(this.modalService));
    }
  }

  protected edit() {
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

  private initializeForm() {
    this.registrationForm = this.formBuilder.group({
      title: ['', Validators.required],
      content: ['', Validators.required],
    });
  }
}
