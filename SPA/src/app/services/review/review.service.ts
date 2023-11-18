import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { DataResponse } from 'src/app/models/dataResponse';
import { ReviewDto } from 'src/app/models/review/reviewDto';
import { ReviewModel } from 'src/app/models/review/reviewModel';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ReviewService {
  private apiUrl: string = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  public postReview(reviewModel: ReviewModel): Observable<unknown> {
    if (!reviewModel.userId) {
      throw new Error('User Id must be defined.', { cause: 'reviewModel' });
    }

    return this.httpClient.post(`${this.apiUrl}Review/Add`, reviewModel);
  }

  public getReviewsPage(page: number, pageSize: number): Observable<ReviewDto[]> {
    return this.httpClient.get<DataResponse<ReviewDto[]>>(
      `${this.apiUrl}Review/Get?page=${page}&pageSize=${pageSize}`
    )
    .pipe(
      map((response: DataResponse<ReviewDto[]>) => response.data)
    )
  }

  public getNumberOfPages(pageSize: number): Observable<number> {
    return this.httpClient.get<DataResponse<number>>(
      `${this.apiUrl}Review/PagesCount?pageSize=${pageSize}`
    )
    .pipe(
      map((response: DataResponse<number>) => response.data)
    )
  }
}
