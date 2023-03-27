import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StoreReviewsServiceProxy, StoreReviewDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreReviewModalComponent } from './create-or-edit-storeReview-modal.component';

import { ViewStoreReviewModalComponent } from './view-storeReview-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './storeReviews.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoreReviewsComponent extends AppComponentBase {
    @ViewChild('createOrEditStoreReviewModal', { static: true })
    createOrEditStoreReviewModal: CreateOrEditStoreReviewModalComponent;
    @ViewChild('viewStoreReviewModal', { static: true }) viewStoreReviewModal: ViewStoreReviewModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    reviewInfoFilter = '';
    maxPostDateFilter: DateTime;
    minPostDateFilter: DateTime;
    postTimeFilter = '';
    isPublishFilter = -1;
    storeNameFilter = '';
    contactFullNameFilter = '';
    ratingLikeNameFilter = '';

    constructor(
        injector: Injector,
        private _storeReviewsServiceProxy: StoreReviewsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getStoreReviews(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeReviewsServiceProxy
            .getAll(
                this.filterText,
                this.reviewInfoFilter,
                this.maxPostDateFilter === undefined
                    ? this.maxPostDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxPostDateFilter),
                this.minPostDateFilter === undefined
                    ? this.minPostDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minPostDateFilter),
                this.postTimeFilter,
                this.isPublishFilter,
                this.storeNameFilter,
                this.contactFullNameFilter,
                this.ratingLikeNameFilter,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createStoreReview(): void {
        this.createOrEditStoreReviewModal.show();
    }

    deleteStoreReview(storeReview: StoreReviewDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._storeReviewsServiceProxy.delete(storeReview.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._storeReviewsServiceProxy
            .getStoreReviewsToExcel(
                this.filterText,
                this.reviewInfoFilter,
                this.maxPostDateFilter === undefined
                    ? this.maxPostDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxPostDateFilter),
                this.minPostDateFilter === undefined
                    ? this.minPostDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minPostDateFilter),
                this.postTimeFilter,
                this.isPublishFilter,
                this.storeNameFilter,
                this.contactFullNameFilter,
                this.ratingLikeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.reviewInfoFilter = '';
        this.maxPostDateFilter = undefined;
        this.minPostDateFilter = undefined;
        this.postTimeFilter = '';
        this.isPublishFilter = -1;
        this.storeNameFilter = '';
        this.contactFullNameFilter = '';
        this.ratingLikeNameFilter = '';

        this.getStoreReviews();
    }
}
