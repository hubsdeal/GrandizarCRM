import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeTagsServiceProxy, EmployeeTagDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditEmployeeTagModalComponent } from './create-or-edit-employeeTag-modal.component';

import { ViewEmployeeTagModalComponent } from './view-employeeTag-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './employeeTags.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class EmployeeTagsComponent extends AppComponentBase {
    @ViewChild('createOrEditEmployeeTagModal', { static: true })
    createOrEditEmployeeTagModal: CreateOrEditEmployeeTagModalComponent;
    @ViewChild('viewEmployeeTagModal', { static: true }) viewEmployeeTagModal: ViewEmployeeTagModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    customTagFilter = '';
    tagValueFilter = '';
    verifiedFilter = -1;
    maxSequenceFilter: number;
    maxSequenceFilterEmpty: number;
    minSequenceFilter: number;
    minSequenceFilterEmpty: number;
    employeeNameFilter = '';
    masterTagCategoryNameFilter = '';
    masterTagNameFilter = '';

    constructor(
        injector: Injector,
        private _employeeTagsServiceProxy: EmployeeTagsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getEmployeeTags(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._employeeTagsServiceProxy
            .getAll(
                this.filterText,
                this.customTagFilter,
                this.tagValueFilter,
                this.verifiedFilter,
                this.maxSequenceFilter == null ? this.maxSequenceFilterEmpty : this.maxSequenceFilter,
                this.minSequenceFilter == null ? this.minSequenceFilterEmpty : this.minSequenceFilter,
                this.employeeNameFilter,
                this.masterTagCategoryNameFilter,
                this.masterTagNameFilter,
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

    createEmployeeTag(): void {
        this.createOrEditEmployeeTagModal.show();
    }

    deleteEmployeeTag(employeeTag: EmployeeTagDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._employeeTagsServiceProxy.delete(employeeTag.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._employeeTagsServiceProxy
            .getEmployeeTagsToExcel(
                this.filterText,
                this.customTagFilter,
                this.tagValueFilter,
                this.verifiedFilter,
                this.maxSequenceFilter == null ? this.maxSequenceFilterEmpty : this.maxSequenceFilter,
                this.minSequenceFilter == null ? this.minSequenceFilterEmpty : this.minSequenceFilter,
                this.employeeNameFilter,
                this.masterTagCategoryNameFilter,
                this.masterTagNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.customTagFilter = '';
        this.tagValueFilter = '';
        this.verifiedFilter = -1;
        this.maxSequenceFilter = this.maxSequenceFilterEmpty;
        this.minSequenceFilter = this.maxSequenceFilterEmpty;
        this.employeeNameFilter = '';
        this.masterTagCategoryNameFilter = '';
        this.masterTagNameFilter = '';

        this.getEmployeeTags();
    }
}
