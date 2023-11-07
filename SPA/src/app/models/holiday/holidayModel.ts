export interface HolidayCreateModel {
  name: string,
  date: Date
}

export interface HolidayEditModel extends HolidayCreateModel {
  id: string
}

export interface HolidayListModel extends HolidayEditModel {}
