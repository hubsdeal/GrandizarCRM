import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BusinessDocumentsServiceProxy, BusinessDocumentDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditBusinessDocumentModalComponent } from './create-or-edit-businessDocument-modal.component';

import { ViewBusinessDocumentModalComponent } from './view-businessDocument-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './businessDocuments.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class BusinessDocumentsComponent extends AppComponentBase {
    @ViewChild('createOrEditBusinessDocumentModal', { static: true })
    createOrEditBusinessDocumentModal: CreateOrEditBusinessDocumentModalComponent;
    @ViewChild('viewBusinessDocumentModal', { static: true })
    viewBusinessDocumentModal: ViewBusinessDocumentModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    documentTitleFilter = '';
    fileBinaryObjectIdFilter = '';
    businessNameFilter = '';
    documentTypeNameFilter = '';

    constructor(
        injector: Injector,
        private _businessDocumentsServiceProxy: BusinessDocumentsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getBusinessDocuments(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._businessDocumentsServiceProxy
            .getAll(
                this.filterText,
                this.documentTitleFilter,
                this.fileBinaryObjectIdFilter,
                this.businessNameFilter,
                this.documentTypeNameFilter,
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

    createBusinessDocument(): void {
        this.createOrEditBusinessDocumentModal.show();
    }

    deleteBusinessDocument(businessDocument: BusinessDocumentDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._businessDocumentsServiceProxy.delete(businessDocument.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._businessDocumentsServiceProxy
            .getBusinessDocumentsToExcel(
                this.filterText,
                this.documentTitleFilter,
                this.fileBinaryObjectIdFilter,
                this.businessNameFilter,
                this.documentTypeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.documentTitleFilter = '';
        this.fileBinaryObjectIdFilter = '';
        this.businessNameFilter = '';
        this.documentTypeNameFilter = '';

        this.getBusinessDocuments();
    }
}
