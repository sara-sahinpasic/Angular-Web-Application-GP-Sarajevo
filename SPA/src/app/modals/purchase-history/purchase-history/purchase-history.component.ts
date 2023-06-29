import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-purchase-history',
  templateUrl: './purchase-history.component.html',
  styleUrls: ['./purchase-history.component.scss'],
})
export class PurchaseHistoryComponent implements OnInit {
  @Input() showModalPurchaseHistory: boolean = false;
  @Output() close: EventEmitter<void> = new EventEmitter<void>();

  constructor() {}
  ngOnInit(): void {}
  purchaseHistoryPrintButton() {}
  historyCloseButton() {
    this.close.emit();
  }
}
