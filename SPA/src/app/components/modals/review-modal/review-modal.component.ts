import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReviewModel } from 'src/app/models/review/reviewModel';
import { LocalizationService } from 'src/app/services/localization/localization.service';
import { ReviewService } from 'src/app/services/review/review.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-review-modal',
  templateUrl: './review-modal.component.html',
  styleUrls: ['./review-modal.component.scss'],
})
export class ReviewModalComponent implements OnInit {
  protected formGroup: FormGroup = {} as FormGroup;
  protected reviewModel: ReviewModel = {
    score: 1,
  } as ReviewModel;
  protected scores: number[] = [1, 2, 3, 4, 5];

  constructor(
    private userService: UserService,
    private reviewService: ReviewService,
    private formBuilder: FormBuilder,
    protected localizationService: LocalizationService,
  ) {}

  ngOnInit() {
    this.initializeForm();
  }

  private initializeForm() {
    this.formGroup = this.formBuilder.group({
      title: ['', Validators.required],
      content: ['', Validators.required]
    });
  }

  protected sendReviewButton() {
    this.formGroup.markAllAsTouched();

    if (this.formGroup.invalid) {
      return;
    }

    this.userService.user$.subscribe((user) => {
      this.reviewModel.userId = user?.id!;
    });

    this.reviewModel.description = this.formGroup.get('content')?.value;
    this.reviewModel.title = this.formGroup.get('title')?.value;

    this.reviewService.postReview(this.reviewModel)
      .subscribe(this.reloadPage);
  }

  private reloadPage() {
    setTimeout(() => {
      location.reload();
    }, 1500);
  }
}
