import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StoreReviewFeedbacksServiceProxy, StoreReviewFeedbackDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreReviewFeedbackModalComponent } from './create-or-edit-storeReviewFeedback-modal.component';

import { ViewStoreReviewFeedbackModalComponent } from './view-storeReviewFeedback-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './storeReviewFeedbacks.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoreReviewFeedbacksComponent extends AppComponentBase {
    @ViewChild('createOrEditStoreReviewFeedbackModal', { static: true })
    createOrEditStoreReviewFeedbackModal: CreateOrEditStoreReviewFeedbackModalComponent;
    @ViewChild('viewStoreReviewFeedbackModal', { static: true })
    viewStoreReviewFeedbackModal: ViewStoreReviewFeedbackModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    replyTextFilter = '';
    isPublishedFilter = -1;
    storeReviewReviewInfoFilter = '';
    contactFullNameFilter = '';
    ratingLikeNameFilter = '';

    constructor(
        injector: Injector,
        private _storeReviewFeedbacksServiceProxy: StoreReviewFeedbacksServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getStoreReviewFeedbacks(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeReviewFeedbacksServiceProxy
            .getAll(
                this.filterText,
                this.replyTextFilter,
                this.isPublishedFilter,
                this.storeReviewReviewInfoFilter,
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

    createStoreReviewFeedback(): void {
        this.createOrEditStoreReviewFeedbackModal.show();
    }

    deleteStoreReviewFeedback(storeReviewFeedback: StoreReviewFeedbackDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._storeReviewFeedbacksServiceProxy.delete(storeReviewFeedback.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._storeReviewFeedbacksServiceProxy
            .getStoreReviewFeedbacksToExcel(
                this.filterText,
                this.replyTextFilter,
                this.isPublishedFilter,
                this.storeReviewReviewInfoFilter,
                this.contactFullNameFilter,
                this.ratingLikeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.replyTextFilter = '';
        this.isPublishedFilter = -1;
        this.storeReviewReviewInfoFilter = '';
        this.contactFullNameFilter = '';
        this.ratingLikeNameFilter = '';

        this.getStoreReviewFeedbacks();
    }
}
