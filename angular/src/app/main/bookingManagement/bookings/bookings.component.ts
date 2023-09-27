import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { BookingsServiceProxy, BookingDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditBookingModalComponent } from './create-or-edit-booking-modal.component';

import { ViewBookingModalComponent } from './view-booking-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './bookings.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class BookingsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditBookingModal', { static: true }) createOrEditBookingModal: CreateOrEditBookingModalComponent;
    @ViewChild('viewBookingModal', { static: true }) viewBookingModal: ViewBookingModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    titleFilter = '';
    bookingReferenceNumberFilter = '';
    maxStartDateFilter : DateTime;
		minStartDateFilter : DateTime;
    maxEndDateFilter : DateTime;
		minEndDateFilter : DateTime;
    startTimeFilter = '';
    endTimeFilter = '';
    timeZoneIdFilter = '';
    descriptionFilter = '';
    customerRemarksFilter = '';
    maxCheckInDateTimeFilter : DateTime;
		minCheckInDateTimeFilter : DateTime;
    maxCheckOutDateTimeFilter : DateTime;
		minCheckOutDateTimeFilter : DateTime;
        bookingTypeNameFilter = '';
        bookingStatusNameFilter = '';
        contactFullNameFilter = '';
        storeNameFilter = '';
        productNameFilter = '';
        orderInvoiceNumberFilter = '';






    constructor(
        injector: Injector,
        private _bookingsServiceProxy: BookingsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getBookings(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._bookingsServiceProxy.getAll(
            this.filterText,
            this.titleFilter,
            this.bookingReferenceNumberFilter,
            this.maxStartDateFilter === undefined ? this.maxStartDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
            this.minStartDateFilter === undefined ? this.minStartDateFilter : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
            this.maxEndDateFilter === undefined ? this.maxEndDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
            this.minEndDateFilter === undefined ? this.minEndDateFilter : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
            this.startTimeFilter,
            this.endTimeFilter,
            this.timeZoneIdFilter,
            this.descriptionFilter,
            this.customerRemarksFilter,
            this.maxCheckInDateTimeFilter === undefined ? this.maxCheckInDateTimeFilter : this._dateTimeService.getEndOfDayForDate(this.maxCheckInDateTimeFilter),
            this.minCheckInDateTimeFilter === undefined ? this.minCheckInDateTimeFilter : this._dateTimeService.getStartOfDayForDate(this.minCheckInDateTimeFilter),
            this.maxCheckOutDateTimeFilter === undefined ? this.maxCheckOutDateTimeFilter : this._dateTimeService.getEndOfDayForDate(this.maxCheckOutDateTimeFilter),
            this.minCheckOutDateTimeFilter === undefined ? this.minCheckOutDateTimeFilter : this._dateTimeService.getStartOfDayForDate(this.minCheckOutDateTimeFilter),
            this.bookingTypeNameFilter,
            this.bookingStatusNameFilter,
            this.contactFullNameFilter,
            this.storeNameFilter,
            this.productNameFilter,
            this.orderInvoiceNumberFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createBooking(): void {
        this.createOrEditBookingModal.show();        
    }


    deleteBooking(booking: BookingDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._bookingsServiceProxy.delete(booking.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._bookingsServiceProxy.getBookingsToExcel(
        this.filterText,
            this.titleFilter,
            this.bookingReferenceNumberFilter,
            this.maxStartDateFilter === undefined ? this.maxStartDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
            this.minStartDateFilter === undefined ? this.minStartDateFilter : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
            this.maxEndDateFilter === undefined ? this.maxEndDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
            this.minEndDateFilter === undefined ? this.minEndDateFilter : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
            this.startTimeFilter,
            this.endTimeFilter,
            this.timeZoneIdFilter,
            this.descriptionFilter,
            this.customerRemarksFilter,
            this.maxCheckInDateTimeFilter === undefined ? this.maxCheckInDateTimeFilter : this._dateTimeService.getEndOfDayForDate(this.maxCheckInDateTimeFilter),
            this.minCheckInDateTimeFilter === undefined ? this.minCheckInDateTimeFilter : this._dateTimeService.getStartOfDayForDate(this.minCheckInDateTimeFilter),
            this.maxCheckOutDateTimeFilter === undefined ? this.maxCheckOutDateTimeFilter : this._dateTimeService.getEndOfDayForDate(this.maxCheckOutDateTimeFilter),
            this.minCheckOutDateTimeFilter === undefined ? this.minCheckOutDateTimeFilter : this._dateTimeService.getStartOfDayForDate(this.minCheckOutDateTimeFilter),
            this.bookingTypeNameFilter,
            this.bookingStatusNameFilter,
            this.contactFullNameFilter,
            this.storeNameFilter,
            this.productNameFilter,
            this.orderInvoiceNumberFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.titleFilter = '';
    this.bookingReferenceNumberFilter = '';
    this.maxStartDateFilter = undefined;
		this.minStartDateFilter = undefined;
    this.maxEndDateFilter = undefined;
		this.minEndDateFilter = undefined;
    this.startTimeFilter = '';
    this.endTimeFilter = '';
    this.timeZoneIdFilter = '';
    this.descriptionFilter = '';
    this.customerRemarksFilter = '';
    this.maxCheckInDateTimeFilter = undefined;
		this.minCheckInDateTimeFilter = undefined;
    this.maxCheckOutDateTimeFilter = undefined;
		this.minCheckOutDateTimeFilter = undefined;
		this.bookingTypeNameFilter = '';
							this.bookingStatusNameFilter = '';
							this.contactFullNameFilter = '';
							this.storeNameFilter = '';
							this.productNameFilter = '';
							this.orderInvoiceNumberFilter = '';
					
        this.getBookings();
    }
}
