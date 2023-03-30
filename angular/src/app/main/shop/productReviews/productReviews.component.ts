import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductReviewsServiceProxy, ProductReviewDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductReviewModalComponent } from './create-or-edit-productReview-modal.component';

import { ViewProductReviewModalComponent } from './view-productReview-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './productReviews.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductReviewsComponent extends AppComponentBase {
    @ViewChild('createOrEditProductReviewModal', { static: true })
    createOrEditProductReviewModal: CreateOrEditProductReviewModalComponent;
    @ViewChild('viewProductReviewModal', { static: true }) viewProductReviewModal: ViewProductReviewModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    reviewInfoFilter = '';
    maxPostDateFilter: DateTime;
    minPostDateFilter: DateTime;
    publishFilter = -1;
    postTimeFilter = '';
    contactFullNameFilter = '';
    productNameFilter = '';
    storeNameFilter = '';
    ratingLikeNameFilter = '';

    constructor(
        injector: Injector,
        private _productReviewsServiceProxy: ProductReviewsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getProductReviews(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productReviewsServiceProxy
            .getAll(
                this.filterText,
                this.reviewInfoFilter,
                this.maxPostDateFilter === undefined
                    ? this.maxPostDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxPostDateFilter),
                this.minPostDateFilter === undefined
                    ? this.minPostDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minPostDateFilter),
                this.publishFilter,
                this.postTimeFilter,
                this.contactFullNameFilter,
                this.productNameFilter,
                this.storeNameFilter,
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

    createProductReview(): void {
        this.createOrEditProductReviewModal.show();
    }

    deleteProductReview(productReview: ProductReviewDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._productReviewsServiceProxy.delete(productReview.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._productReviewsServiceProxy
            .getProductReviewsToExcel(
                this.filterText,
                this.reviewInfoFilter,
                this.maxPostDateFilter === undefined
                    ? this.maxPostDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxPostDateFilter),
                this.minPostDateFilter === undefined
                    ? this.minPostDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minPostDateFilter),
                this.publishFilter,
                this.postTimeFilter,
                this.contactFullNameFilter,
                this.productNameFilter,
                this.storeNameFilter,
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
        this.publishFilter = -1;
        this.postTimeFilter = '';
        this.contactFullNameFilter = '';
        this.productNameFilter = '';
        this.storeNameFilter = '';
        this.ratingLikeNameFilter = '';

        this.getProductReviews();
    }
}
