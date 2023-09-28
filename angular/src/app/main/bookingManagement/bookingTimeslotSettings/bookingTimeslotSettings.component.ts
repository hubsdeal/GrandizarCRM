import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { BookingTimeslotSettingsServiceProxy, BookingTimeslotSettingDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditBookingTimeslotSettingModalComponent } from './create-or-edit-bookingTimeslotSetting-modal.component';

import { ViewBookingTimeslotSettingModalComponent } from './view-bookingTimeslotSetting-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './bookingTimeslotSettings.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class BookingTimeslotSettingsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditBookingTimeslotSettingModal', { static: true }) createOrEditBookingTimeslotSettingModal: CreateOrEditBookingTimeslotSettingModalComponent;
    @ViewChild('viewBookingTimeslotSettingModal', { static: true }) viewBookingTimeslotSettingModal: ViewBookingTimeslotSettingModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    timezoneIdFilter = '';
    maxStartDateFilter : DateTime;
		minStartDateFilter : DateTime;
    maxEndDateFilter : DateTime;
		minEndDateFilter : DateTime;
    startTimeFilter = '';
    endTimeFilter = '';
    publiclyPublishedFilter = -1;
    termsAndPolicyFilter = '';
    automaticOrManualApprovalFilter = -1;
    storeTeamNotificationFilter = -1;
    maxCancellationDeadlineHoursFilter : number;
		maxCancellationDeadlineHoursFilterEmpty : number;
		minCancellationDeadlineHoursFilter : number;
		minCancellationDeadlineHoursFilterEmpty : number;
        storeNameFilter = '';
        productNameFilter = '';
        bookingTypeNameFilter = '';
        bookingStatusNameFilter = '';






    constructor(
        injector: Injector,
        private _bookingTimeslotSettingsServiceProxy: BookingTimeslotSettingsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getBookingTimeslotSettings(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._bookingTimeslotSettingsServiceProxy.getAll(
            this.filterText,
            this.timezoneIdFilter,
            this.maxStartDateFilter === undefined ? this.maxStartDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
            this.minStartDateFilter === undefined ? this.minStartDateFilter : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
            this.maxEndDateFilter === undefined ? this.maxEndDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
            this.minEndDateFilter === undefined ? this.minEndDateFilter : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
            this.startTimeFilter,
            this.endTimeFilter,
            this.publiclyPublishedFilter,
            this.termsAndPolicyFilter,
            this.automaticOrManualApprovalFilter,
            this.storeTeamNotificationFilter,
            this.maxCancellationDeadlineHoursFilter == null ? this.maxCancellationDeadlineHoursFilterEmpty: this.maxCancellationDeadlineHoursFilter,
            this.minCancellationDeadlineHoursFilter == null ? this.minCancellationDeadlineHoursFilterEmpty: this.minCancellationDeadlineHoursFilter,
            this.storeNameFilter,
            this.productNameFilter,
            this.bookingTypeNameFilter,
            this.bookingStatusNameFilter,
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

    createBookingTimeslotSetting(): void {
        this.createOrEditBookingTimeslotSettingModal.show();        
    }


    deleteBookingTimeslotSetting(bookingTimeslotSetting: BookingTimeslotSettingDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._bookingTimeslotSettingsServiceProxy.delete(bookingTimeslotSetting.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._bookingTimeslotSettingsServiceProxy.getBookingTimeslotSettingsToExcel(
        this.filterText,
            this.timezoneIdFilter,
            this.maxStartDateFilter === undefined ? this.maxStartDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
            this.minStartDateFilter === undefined ? this.minStartDateFilter : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
            this.maxEndDateFilter === undefined ? this.maxEndDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
            this.minEndDateFilter === undefined ? this.minEndDateFilter : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
            this.startTimeFilter,
            this.endTimeFilter,
            this.publiclyPublishedFilter,
            this.termsAndPolicyFilter,
            this.automaticOrManualApprovalFilter,
            this.storeTeamNotificationFilter,
            this.maxCancellationDeadlineHoursFilter == null ? this.maxCancellationDeadlineHoursFilterEmpty: this.maxCancellationDeadlineHoursFilter,
            this.minCancellationDeadlineHoursFilter == null ? this.minCancellationDeadlineHoursFilterEmpty: this.minCancellationDeadlineHoursFilter,
            this.storeNameFilter,
            this.productNameFilter,
            this.bookingTypeNameFilter,
            this.bookingStatusNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.timezoneIdFilter = '';
    this.maxStartDateFilter = undefined;
		this.minStartDateFilter = undefined;
    this.maxEndDateFilter = undefined;
		this.minEndDateFilter = undefined;
    this.startTimeFilter = '';
    this.endTimeFilter = '';
    this.publiclyPublishedFilter = -1;
    this.termsAndPolicyFilter = '';
    this.automaticOrManualApprovalFilter = -1;
    this.storeTeamNotificationFilter = -1;
    this.maxCancellationDeadlineHoursFilter = this.maxCancellationDeadlineHoursFilterEmpty;
		this.minCancellationDeadlineHoursFilter = this.maxCancellationDeadlineHoursFilterEmpty;
		this.storeNameFilter = '';
							this.productNameFilter = '';
							this.bookingTypeNameFilter = '';
							this.bookingStatusNameFilter = '';
					
        this.getBookingTimeslotSettings();
    }
}
