import { Component } from '@angular/core';
import { ReviewModel } from 'src/app/models/Review/ReviewModel';
import { ReviewService } from 'src/app/services/review/review.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-review-modal',
  templateUrl: './review-modal.component.html',
  styleUrls: ['./review-modal.component.scss'],
})
export class ReviewModalComponent {
  constructor(
    private userService: UserService,
    private reviewService: ReviewService
  ) {}

  public reviewModel: ReviewModel = {
    title: '',
    description: '',
    score: 1,
    userId: '',
  };
  reviewModels: Array<number> = [1, 2, 3, 4, 5];

  sendReviewButton() {
    this.userService.user$.subscribe((user) => {
      this.reviewModel.userId = user?.id;
    });

    this.reviewService.postReview(this.reviewModel).subscribe(() => {
      location.reload();
    });
  }
}
