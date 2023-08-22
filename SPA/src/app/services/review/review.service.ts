import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DataResponse } from 'src/app/models/DataResponse';
import { ReviewDto } from 'src/app/models/Review/ReviewDto';
import { ReviewModel } from 'src/app/models/Review/ReviewModel';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ReviewService {
  private apiUrl: string = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  public getAllReviews(): Observable<DataResponse<ReviewDto[]>> {
    return this.httpClient.get<DataResponse<ReviewDto[]>>(
      `${this.apiUrl}Review/GetAllReviews`
    );
  }

  public postReview(
    reviewModel: ReviewModel
  ): Observable<DataResponse<ReviewDto[]>> {
    return this.httpClient.post<DataResponse<ReviewDto[]>>(
      `${this.apiUrl}Review/AddReview`,
      reviewModel
    );
  }
}
