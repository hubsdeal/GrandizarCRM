import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BusinessTaskMapsServiceProxy, BusinessTaskMapDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditBusinessTaskMapModalComponent } from './create-or-edit-businessTaskMap-modal.component';

import { ViewBusinessTaskMapModalComponent } from './view-businessTaskMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './businessTaskMaps.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class BusinessTaskMapsComponent extends AppComponentBase {
    @ViewChild('createOrEditBusinessTaskMapModal', { static: true })
    createOrEditBusinessTaskMapModal: CreateOrEditBusinessTaskMapModalComponent;
    @ViewChild('viewBusinessTaskMapModal', { static: true })
    viewBusinessTaskMapModal: ViewBusinessTaskMapModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    businessNameFilter = '';
    taskEventNameFilter = '';

    constructor(
        injector: Injector,
        private _businessTaskMapsServiceProxy: BusinessTaskMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getBusinessTaskMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._businessTaskMapsServiceProxy
            .getAll(
                this.filterText,
                this.businessNameFilter,
                this.taskEventNameFilter,
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

    createBusinessTaskMap(): void {
        this.createOrEditBusinessTaskMapModal.show();
    }

    deleteBusinessTaskMap(businessTaskMap: BusinessTaskMapDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._businessTaskMapsServiceProxy.delete(businessTaskMap.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._businessTaskMapsServiceProxy
            .getBusinessTaskMapsToExcel(this.filterText, this.businessNameFilter, this.taskEventNameFilter)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.businessNameFilter = '';
        this.taskEventNameFilter = '';

        this.getBusinessTaskMaps();
    }
}
