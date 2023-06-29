import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-request',
  templateUrl: './request.component.html',
  styleUrls: ['./request.component.scss'],
})
export class RequestComponent implements OnInit {
  @Input() showModalRequest: boolean = false;
  @Output() close: EventEmitter<void> = new EventEmitter<void>();

  constructor(private router:Router, private httpClient:HttpClient) {}

  private url: string = environment.apiUrl;
  requests: Array<any>=[];
  request_id?:number;

  ngOnInit(): void {
    this.httpClient.get(`${this.url}`)
      .subscribe((r: any) => {
        this.requests = r;
      });
  }

  requestCloseButton() {
    this.close.emit();
  }
  sendRequestButton() {}
}
