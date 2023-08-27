import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReviewModel } from 'src/app/models/Review/ReviewModel';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { ReviewService } from 'src/app/services/review/review.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-review-modal',
  templateUrl: './review-modal.component.html',
  styleUrls: ['./review-modal.component.scss'],
})
export class ReviewModalComponent implements OnInit {

  protected formGroup!: FormGroup;

  constructor(
    private userService: UserService,
    private reviewService: ReviewService,
    private formBuilder: FormBuilder,
    protected localizationService: LocalizationService,
  ) {}

  ngOnInit(): void {
    this.inizializeValidators();
  }

  inizializeValidators() {
    this.formGroup = this.formBuilder.group({
      title: ["", Validators.required],
      content: ["", Validators.required]
    });
  }

  public reviewModel: ReviewModel = {
    title: '',
    description: '',
    score: 1,
    userId: '',
  };
  reviewModels: Array<number> = [1, 2, 3, 4, 5];

  sendReviewButton() {
    this.formGroup.markAllAsTouched();

    if (this.formGroup.invalid) {
      return;
    }

    this.userService.user$.subscribe((user) => {
      this.reviewModel.userId = user?.id;
    });

    this.reviewModel.description = this.formGroup.get("content")?.value;
    this.reviewModel.title = this.formGroup.get("title")?.value;

    this.reviewService.postReview(this.reviewModel).subscribe(() => {
      location.reload();
    });
  }
}
