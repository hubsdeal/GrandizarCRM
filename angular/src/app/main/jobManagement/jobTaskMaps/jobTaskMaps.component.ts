import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { JobTaskMapsServiceProxy, JobTaskMapDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditJobTaskMapModalComponent } from './create-or-edit-jobTaskMap-modal.component';

import { ViewJobTaskMapModalComponent } from './view-jobTaskMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './jobTaskMaps.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class JobTaskMapsComponent extends AppComponentBase {
    @ViewChild('createOrEditJobTaskMapModal', { static: true })
    createOrEditJobTaskMapModal: CreateOrEditJobTaskMapModalComponent;
    @ViewChild('viewJobTaskMapModal', { static: true }) viewJobTaskMapModal: ViewJobTaskMapModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    jobTitleFilter = '';
    taskEventNameFilter = '';

    constructor(
        injector: Injector,
        private _jobTaskMapsServiceProxy: JobTaskMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getJobTaskMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._jobTaskMapsServiceProxy
            .getAll(
                this.filterText,
                this.jobTitleFilter,
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

    createJobTaskMap(): void {
        this.createOrEditJobTaskMapModal.show();
    }

    deleteJobTaskMap(jobTaskMap: JobTaskMapDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._jobTaskMapsServiceProxy.delete(jobTaskMap.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._jobTaskMapsServiceProxy
            .getJobTaskMapsToExcel(this.filterText, this.jobTitleFilter, this.taskEventNameFilter)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.jobTitleFilter = '';
        this.taskEventNameFilter = '';

        this.getJobTaskMaps();
    }
}
