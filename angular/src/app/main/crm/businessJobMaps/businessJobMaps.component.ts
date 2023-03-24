import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BusinessJobMapsServiceProxy, BusinessJobMapDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditBusinessJobMapModalComponent } from './create-or-edit-businessJobMap-modal.component';

import { ViewBusinessJobMapModalComponent } from './view-businessJobMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './businessJobMaps.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class BusinessJobMapsComponent extends AppComponentBase {
    @ViewChild('createOrEditBusinessJobMapModal', { static: true })
    createOrEditBusinessJobMapModal: CreateOrEditBusinessJobMapModalComponent;
    @ViewChild('viewBusinessJobMapModal', { static: true }) viewBusinessJobMapModal: ViewBusinessJobMapModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    businessNameFilter = '';
    jobTitleFilter = '';

    constructor(
        injector: Injector,
        private _businessJobMapsServiceProxy: BusinessJobMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getBusinessJobMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._businessJobMapsServiceProxy
            .getAll(
                this.filterText,
                this.businessNameFilter,
                this.jobTitleFilter,
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

    createBusinessJobMap(): void {
        this.createOrEditBusinessJobMapModal.show();
    }

    deleteBusinessJobMap(businessJobMap: BusinessJobMapDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._businessJobMapsServiceProxy.delete(businessJobMap.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._businessJobMapsServiceProxy
            .getBusinessJobMapsToExcel(this.filterText, this.businessNameFilter, this.jobTitleFilter)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.businessNameFilter = '';
        this.jobTitleFilter = '';

        this.getBusinessJobMaps();
    }
}
