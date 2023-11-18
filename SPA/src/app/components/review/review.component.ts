import { Component, OnInit } from '@angular/core';
import { tap } from 'rxjs';
import { Pagination } from 'src/app/models/pagination/pagination';
import { ReviewDto } from 'src/app/models/review/reviewDto';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { ModalService } from 'src/app/services/modal/modal.service';
import { ReviewService } from 'src/app/services/review/review.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-review',
  templateUrl: './review.component.html',
  styleUrls: ['./review.component.scss'],
})
export class ReviewComponent implements OnInit {
  protected shouldShowReviewButton: boolean = false;
  protected reviews: ReviewDto[] = [];
  protected paginationModel: Pagination = {
    pageSize: 4,
    page: 1,
  };
  protected pages: number = 0;
  protected pageNumbers: number[] = [];
  protected scoreMarkerColors: string[] = ['#C41E3A', '#FCF55F', '#009E60'];

  constructor(
    private modalService: ModalService,
    private userService: UserService,
    private reviewService: ReviewService,
    protected localizationService: LocalizationService
  ) {}

  protected getMarkerColor(score: number): string {
    return this.scoreMarkerColors[Math.floor(score / 2)];
  }

  ngOnInit() {
    this.isUserLoggedIn();
    this.getPageSize();
    this.getReviewsForPage();
  }

  protected isUserLoggedIn() {
    this.shouldShowReviewButton = this.userService.isLoggedIn();
  }

  protected showReviewModalButton() {
    this.modalService.showReviewModal();
  }

  protected getReviewsForPage() {
    this.reviewService.getReviewsPage(this.paginationModel.page, this.paginationModel.pageSize)
      .pipe(
        tap((response: ReviewDto[]) => this.reviews = response)
      )
      .subscribe();
  }

  protected nextPage() {
    if (this.paginationModel.page >= this.pages) {
      return;
    }

    this.paginationModel.page++;
    this.getReviewsForPage();
  }

  protected prevPage() {
    if (this.paginationModel.page <= 1) {
      return;
    }

    this.paginationModel.page--;
    this.getReviewsForPage();
  }

  protected getPageSize() {
   this.reviewService.getNumberOfPages(this.paginationModel.pageSize)
      .pipe(
        tap((response: number) => {
        this.pages = response;
        this.pageNumbers = Array(this.pages).fill(1)
          .map((x, i) => x + i);
      }))
      .subscribe();
  }

  protected changePage(page: number) {
    this.paginationModel.page = page;
    this.getReviewsForPage();
  }
}
