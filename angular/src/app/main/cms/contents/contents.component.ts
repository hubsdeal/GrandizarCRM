import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ContentsServiceProxy, ContentDto, ContentType } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditContentModalComponent } from './create-or-edit-content-modal.component';

import { ViewContentModalComponent } from './view-content-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './contents.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ContentsComponent extends AppComponentBase {
    @ViewChild('createOrEditContentModal', { static: true })
    createOrEditContentModal: CreateOrEditContentModalComponent;
    @ViewChild('viewContentModal', { static: true }) viewContentModal: ViewContentModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    titleFilter = '';
    bodyFilter = '';
    publishedFilter = -1;
    maxPublishedDateFilter: DateTime;
    minPublishedDateFilter: DateTime;
    publishTimeFilter = '';
    seoUrlFilter = '';
    seoKeywordsFilter = '';
    seoDescriptionFilter = '';
    contentTypeIdFilter = -1;
    mediaLibraryNameFilter = '';

    contentType = ContentType;

    constructor(
        injector: Injector,
        private _contentsServiceProxy: ContentsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getContents(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._contentsServiceProxy
            .getAll(
                this.filterText,
                this.titleFilter,
                this.bodyFilter,
                this.publishedFilter,
                this.maxPublishedDateFilter === undefined
                    ? this.maxPublishedDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxPublishedDateFilter),
                this.minPublishedDateFilter === undefined
                    ? this.minPublishedDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minPublishedDateFilter),
                this.publishTimeFilter,
                this.seoUrlFilter,
                this.seoKeywordsFilter,
                this.seoDescriptionFilter,
                this.contentTypeIdFilter,
                this.mediaLibraryNameFilter,
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

    createContent(): void {
        this.createOrEditContentModal.show();
    }

    deleteContent(content: ContentDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._contentsServiceProxy.delete(content.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._contentsServiceProxy
            .getContentsToExcel(
                this.filterText,
                this.titleFilter,
                this.bodyFilter,
                this.publishedFilter,
                this.maxPublishedDateFilter === undefined
                    ? this.maxPublishedDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxPublishedDateFilter),
                this.minPublishedDateFilter === undefined
                    ? this.minPublishedDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minPublishedDateFilter),
                this.publishTimeFilter,
                this.seoUrlFilter,
                this.seoKeywordsFilter,
                this.seoDescriptionFilter,
                this.contentTypeIdFilter,
                this.mediaLibraryNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.titleFilter = '';
        this.bodyFilter = '';
        this.publishedFilter = -1;
        this.maxPublishedDateFilter = undefined;
        this.minPublishedDateFilter = undefined;
        this.publishTimeFilter = '';
        this.seoUrlFilter = '';
        this.seoKeywordsFilter = '';
        this.seoDescriptionFilter = '';
        this.contentTypeIdFilter = -1;
        this.mediaLibraryNameFilter = '';

        this.getContents();
    }
}
