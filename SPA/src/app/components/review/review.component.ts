import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DataResponse } from 'src/app/models/DataResponse';
import { Pagination } from 'src/app/models/Pagination/Pagination';
import { ReviewDto } from 'src/app/models/Review/ReviewDto';
import { ModalService } from 'src/app/services/modal/modal.service';
import { UserService } from 'src/app/services/user/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-review',
  templateUrl: './review.component.html',
  styleUrls: ['./review.component.scss'],
})
export class ReviewComponent implements OnInit {

  shouldShowReviewButton: boolean = false;
  protected reviewTable: Array<ReviewDto> = [];
  public paginationModel: Pagination = {
    pageSize: 4,
    page: 1,
  };
  pages: number = 0;
  pagesArray: Array<number> = [];
  private apiUrl: string = environment.apiUrl;
  protected scoreMarkerColors: string[] = ["#C41E3A", "#FCF55F", "#009E60"];

  constructor(
    private modalService: ModalService,
    private userService: UserService,
    private httpClient: HttpClient
  ) {}

  getMarkerColor(score: number): string {
    return this.scoreMarkerColors[Math.floor(score / 2)];
  }

  ngOnInit(): void {
    this.isUserLoggedIn();
    this.getPageSize();
    this.getPage();
  }

  isUserLoggedIn() {
    this.userService.user$.subscribe((user: any) => {
      this.shouldShowReviewButton = !!user;
    });
  }

  showReviewModalButton() {
    this.modalService.showReviewModal();
  }

  getPage() {
    this.httpClient
      .get(
        `${this.apiUrl}Review/Pagination?page=${this.paginationModel.page}&pageSize=${this.paginationModel.pageSize}`
      )
      .subscribe((x: any) => {
        this.reviewTable = x;
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
        `${this.apiUrl}Review/ReviewPagesCount?pageSize=${this.paginationModel.pageSize}`
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
