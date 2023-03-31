import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductReviewFeedbacksServiceProxy, ProductReviewFeedbackDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductReviewFeedbackModalComponent } from './create-or-edit-productReviewFeedback-modal.component';

import { ViewProductReviewFeedbackModalComponent } from './view-productReviewFeedback-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './productReviewFeedbacks.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductReviewFeedbacksComponent extends AppComponentBase {
    @ViewChild('createOrEditProductReviewFeedbackModal', { static: true })
    createOrEditProductReviewFeedbackModal: CreateOrEditProductReviewFeedbackModalComponent;
    @ViewChild('viewProductReviewFeedbackModal', { static: true })
    viewProductReviewFeedbackModal: ViewProductReviewFeedbackModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    replyTextFilter = '';
    publishedFilter = -1;
    contactFullNameFilter = '';
    productReviewReviewInfoFilter = '';
    ratingLikeNameFilter = '';

    constructor(
        injector: Injector,
        private _productReviewFeedbacksServiceProxy: ProductReviewFeedbacksServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getProductReviewFeedbacks(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productReviewFeedbacksServiceProxy
            .getAll(
                this.filterText,
                this.replyTextFilter,
                this.publishedFilter,
                this.contactFullNameFilter,
                this.productReviewReviewInfoFilter,
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

    createProductReviewFeedback(): void {
        this.createOrEditProductReviewFeedbackModal.show();
    }

    deleteProductReviewFeedback(productReviewFeedback: ProductReviewFeedbackDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._productReviewFeedbacksServiceProxy.delete(productReviewFeedback.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._productReviewFeedbacksServiceProxy
            .getProductReviewFeedbacksToExcel(
                this.filterText,
                this.replyTextFilter,
                this.publishedFilter,
                this.contactFullNameFilter,
                this.productReviewReviewInfoFilter,
                this.ratingLikeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.replyTextFilter = '';
        this.publishedFilter = -1;
        this.contactFullNameFilter = '';
        this.productReviewReviewInfoFilter = '';
        this.ratingLikeNameFilter = '';

        this.getProductReviewFeedbacks();
    }
}
