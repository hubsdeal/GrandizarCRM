import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MasterWidgetsServiceProxy, MasterWidgetDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditMasterWidgetModalComponent } from './create-or-edit-masterWidget-modal.component';

import { ViewMasterWidgetModalComponent } from './view-masterWidget-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './masterWidgets.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class MasterWidgetsComponent extends AppComponentBase {
    @ViewChild('createOrEditMasterWidgetModal', { static: true })
    createOrEditMasterWidgetModal: CreateOrEditMasterWidgetModalComponent;
    @ViewChild('viewMasterWidgetModal', { static: true }) viewMasterWidgetModal: ViewMasterWidgetModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    descriptionFilter = '';
    designCodeFilter = '';
    publishFilter = -1;
    maxInternalDisplayNumberFilter: number;
    maxInternalDisplayNumberFilterEmpty: number;
    minInternalDisplayNumberFilter: number;
    minInternalDisplayNumberFilterEmpty: number;
    thumbnailImageIdFilter = '';

    constructor(
        injector: Injector,
        private _masterWidgetsServiceProxy: MasterWidgetsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getMasterWidgets(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._masterWidgetsServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.descriptionFilter,
                this.designCodeFilter,
                this.publishFilter,
                this.maxInternalDisplayNumberFilter == null
                    ? this.maxInternalDisplayNumberFilterEmpty
                    : this.maxInternalDisplayNumberFilter,
                this.minInternalDisplayNumberFilter == null
                    ? this.minInternalDisplayNumberFilterEmpty
                    : this.minInternalDisplayNumberFilter,
                this.thumbnailImageIdFilter,
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

    createMasterWidget(): void {
        this.createOrEditMasterWidgetModal.show();
    }

    deleteMasterWidget(masterWidget: MasterWidgetDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._masterWidgetsServiceProxy.delete(masterWidget.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._masterWidgetsServiceProxy
            .getMasterWidgetsToExcel(
                this.filterText,
                this.nameFilter,
                this.descriptionFilter,
                this.designCodeFilter,
                this.publishFilter,
                this.maxInternalDisplayNumberFilter == null
                    ? this.maxInternalDisplayNumberFilterEmpty
                    : this.maxInternalDisplayNumberFilter,
                this.minInternalDisplayNumberFilter == null
                    ? this.minInternalDisplayNumberFilterEmpty
                    : this.minInternalDisplayNumberFilter,
                this.thumbnailImageIdFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.nameFilter = '';
        this.descriptionFilter = '';
        this.designCodeFilter = '';
        this.publishFilter = -1;
        this.maxInternalDisplayNumberFilter = this.maxInternalDisplayNumberFilterEmpty;
        this.minInternalDisplayNumberFilter = this.maxInternalDisplayNumberFilterEmpty;
        this.thumbnailImageIdFilter = '';

        this.getMasterWidgets();
    }
}
