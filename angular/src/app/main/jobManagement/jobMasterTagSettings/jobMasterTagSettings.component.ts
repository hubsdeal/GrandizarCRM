﻿import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    JobMasterTagSettingsServiceProxy,
    JobMasterTagSettingDto,
    AnswerType,
} from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditJobMasterTagSettingModalComponent } from './create-or-edit-jobMasterTagSetting-modal.component';

import { ViewJobMasterTagSettingModalComponent } from './view-jobMasterTagSetting-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './jobMasterTagSettings.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class JobMasterTagSettingsComponent extends AppComponentBase {
    @ViewChild('createOrEditJobMasterTagSettingModal', { static: true })
    createOrEditJobMasterTagSettingModal: CreateOrEditJobMasterTagSettingModalComponent;
    @ViewChild('viewJobMasterTagSettingModal', { static: true })
    viewJobMasterTagSettingModal: ViewJobMasterTagSettingModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxDisplaySequenceFilter: number;
    maxDisplaySequenceFilterEmpty: number;
    minDisplaySequenceFilter: number;
    minDisplaySequenceFilterEmpty: number;
    displayPublicFilter = -1;
    answerTypeIdFilter = -1;
    customTagTitleFilter = '';
    customTagChatQuestionFilter = '';
    masterTagNameFilter = '';
    masterTagCategoryNameFilter = '';

    answerType = AnswerType;

    constructor(
        injector: Injector,
        private _jobMasterTagSettingsServiceProxy: JobMasterTagSettingsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getJobMasterTagSettings(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._jobMasterTagSettingsServiceProxy
            .getAll(
                this.filterText,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.displayPublicFilter,
                this.answerTypeIdFilter,
                this.customTagTitleFilter,
                this.customTagChatQuestionFilter,
                this.masterTagNameFilter,
                this.masterTagCategoryNameFilter,
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

    createJobMasterTagSetting(): void {
        this.createOrEditJobMasterTagSettingModal.show();
    }

    deleteJobMasterTagSetting(jobMasterTagSetting: JobMasterTagSettingDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._jobMasterTagSettingsServiceProxy.delete(jobMasterTagSetting.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._jobMasterTagSettingsServiceProxy
            .getJobMasterTagSettingsToExcel(
                this.filterText,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.displayPublicFilter,
                this.answerTypeIdFilter,
                this.customTagTitleFilter,
                this.customTagChatQuestionFilter,
                this.masterTagNameFilter,
                this.masterTagCategoryNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
        this.minDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
        this.displayPublicFilter = -1;
        this.answerTypeIdFilter = -1;
        this.customTagTitleFilter = '';
        this.customTagChatQuestionFilter = '';
        this.masterTagNameFilter = '';
        this.masterTagCategoryNameFilter = '';

        this.getJobMasterTagSettings();
    }
}
