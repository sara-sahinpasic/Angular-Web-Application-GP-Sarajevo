import { Component } from '@angular/core';
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { tap } from 'rxjs';
import { InvalidArgumentException } from 'src/app/exceptions/InvalidArgumentException';
import { Pagination } from 'src/app/models/Pagination/Pagination';
import { HolidayListModel } from 'src/app/models/holiday/holidayModel';
import { HolidayService } from 'src/app/services/holiday/holiday.service';
import { ModalService } from 'src/app/services/modal/modal.service';

@Component({
  selector: 'app-admin-holidays-page',
  templateUrl: './admin-holidays-page.component.html',
  styleUrls: ['./admin-holidays-page.component.scss']
})
export class AdminHolidaysPageComponent {
  protected paginationModel: Pagination = {
    pageSize: 5,
    page: 1,
    totalCount: 0,
  };
  protected holidayList: HolidayListModel[] = [];
  protected filteredHolidayList: HolidayListModel[] = [];
  protected editIcon = faEdit;
  protected deleteIcon = faTrash;

  constructor(private holidayService: HolidayService, private modalService: ModalService) {}

  ngOnInit() {
    this.getListData();
  }

  private getListData() {
    this.holidayService.getHolidayList()
      .pipe(
        tap(this.loadHolidays.bind(this))
      )
      .subscribe();
  }

  private loadHolidays(data: HolidayListModel[]) {
    this.holidayList = data;
    this.filteredHolidayList = data;
  }

  protected onPageChange(event) {
    this.paginationModel.page = event;
  }

  protected showHolidayEditModal(holiday: HolidayListModel) {
    this.modalService.data = holiday;
    this.modalService.adminShowHolidayModal();
  }

  protected deleteHoliday(holiday: HolidayListModel) {
    if (!holiday.id) {
      throw new InvalidArgumentException(['holiday'])
    }

    this.holidayService.deleteHoliday(holiday.id)
      .subscribe(this.reloadPage);
  }

  private reloadPage() {
    setTimeout(() => {
      location.reload();
    }, 1500);
  }

  protected findByName(event: KeyboardEvent) {
    const target: HTMLInputElement = event.target as HTMLInputElement;

    if (!target.value) {
      this.resetFilter();
      return;
    }

    this.filteredHolidayList = this.holidayList.filter(holiday => holiday.name.toLowerCase().indexOf(target.value.toLowerCase()) > -1);
  }

  private resetFilter() {
    this.filteredHolidayList = this.holidayList;
  }
}
