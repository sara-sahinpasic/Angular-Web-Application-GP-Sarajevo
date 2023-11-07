import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ReviewModel } from 'src/app/models/Review/ReviewModel';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ReviewService {
  private apiUrl: string = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  public postReview(reviewModel: ReviewModel): Observable<unknown> {
    return this.httpClient.post(`${this.apiUrl}Review/AddReview`, reviewModel);
  }
}
