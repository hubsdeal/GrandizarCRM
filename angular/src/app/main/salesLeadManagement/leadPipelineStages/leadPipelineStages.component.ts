import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LeadPipelineStagesServiceProxy, LeadPipelineStageDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditLeadPipelineStageModalComponent } from './create-or-edit-leadPipelineStage-modal.component';

import { ViewLeadPipelineStageModalComponent } from './view-leadPipelineStage-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './leadPipelineStages.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class LeadPipelineStagesComponent extends AppComponentBase {
    @ViewChild('createOrEditLeadPipelineStageModal', { static: true })
    createOrEditLeadPipelineStageModal: CreateOrEditLeadPipelineStageModalComponent;
    @ViewChild('viewLeadPipelineStageModal', { static: true })
    viewLeadPipelineStageModal: ViewLeadPipelineStageModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    maxStageOrderFilter: number;
    maxStageOrderFilterEmpty: number;
    minStageOrderFilter: number;
    minStageOrderFilterEmpty: number;

    constructor(
        injector: Injector,
        private _leadPipelineStagesServiceProxy: LeadPipelineStagesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getLeadPipelineStages(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._leadPipelineStagesServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.maxStageOrderFilter == null ? this.maxStageOrderFilterEmpty : this.maxStageOrderFilter,
                this.minStageOrderFilter == null ? this.minStageOrderFilterEmpty : this.minStageOrderFilter,
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

    createLeadPipelineStage(): void {
        this.createOrEditLeadPipelineStageModal.show();
    }

    deleteLeadPipelineStage(leadPipelineStage: LeadPipelineStageDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._leadPipelineStagesServiceProxy.delete(leadPipelineStage.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._leadPipelineStagesServiceProxy
            .getLeadPipelineStagesToExcel(
                this.filterText,
                this.nameFilter,
                this.maxStageOrderFilter == null ? this.maxStageOrderFilterEmpty : this.maxStageOrderFilter,
                this.minStageOrderFilter == null ? this.minStageOrderFilterEmpty : this.minStageOrderFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.nameFilter = '';
        this.maxStageOrderFilter = this.maxStageOrderFilterEmpty;
        this.minStageOrderFilter = this.maxStageOrderFilterEmpty;

        this.getLeadPipelineStages();
    }
}
