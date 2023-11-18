import { formatDate } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HolidayCreateModel, HolidayEditModel } from 'src/app/models/holiday/holidayModel';
import { HolidayService } from 'src/app/services/holiday/holiday.service';
import { ModalService } from 'src/app/services/modal/modal.service';

@Component({
  selector: 'app-admin-holiday-modal',
  templateUrl: './admin-holiday-modal.component.html',
  styleUrls: ['./admin-holiday-modal.component.scss']
})
export class AdminHolidayModalComponent implements OnInit {
  @Input() holidayToEdit?: HolidayEditModel;
  protected holidayFormGroup!: FormGroup;

  constructor(private holidayService: HolidayService, private modalService: ModalService,
    private formBuilder: FormBuilder) {}

  public ngOnInit() {
    this.initializeFormGoup();
  }

  private initializeFormGoup() {
    this.holidayFormGroup = this.formBuilder.group({
      name: [this.holidayToEdit?.name ?? '', Validators.required],
      date: [this.holidayToEdit?.date ? formatDate(this.holidayToEdit!.date, 'yyyy-MM-dd', 'en') : null, Validators.required]
    });
  }

  protected saveHoliday() {
    this.validateForm();

    if (this.holidayFormGroup.invalid) {
      return;
    }

    const holidayToCreate: HolidayCreateModel = this.holidayFormGroup.value as HolidayCreateModel;

    this.holidayService.createHoliday(holidayToCreate)
      .subscribe(this.modalService.closeModal.bind(this.modalService));
  }

  protected editHoliday() {
    this.validateForm();

    if (this.holidayFormGroup.invalid) {
      return;
    }

    const holidayToEdit: HolidayEditModel = this.holidayFormGroup.value as HolidayEditModel;
    holidayToEdit.id = this.holidayToEdit!.id;

    this.holidayService.editHoliday(holidayToEdit)
      .subscribe(this.reloadPage);
  }

  private reloadPage() {
    setTimeout(() => location.reload(), 1500);
  }

  private validateForm() {
    this.holidayFormGroup.markAllAsTouched();
  }
}
