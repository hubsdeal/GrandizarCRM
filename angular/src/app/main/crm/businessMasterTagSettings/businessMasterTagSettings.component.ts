import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    BusinessMasterTagSettingsServiceProxy,
    BusinessMasterTagSettingDto,
    AnswerType,
} from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditBusinessMasterTagSettingModalComponent } from './create-or-edit-businessMasterTagSetting-modal.component';

import { ViewBusinessMasterTagSettingModalComponent } from './view-businessMasterTagSetting-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './businessMasterTagSettings.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class BusinessMasterTagSettingsComponent extends AppComponentBase {
    @ViewChild('createOrEditBusinessMasterTagSettingModal', { static: true })
    createOrEditBusinessMasterTagSettingModal: CreateOrEditBusinessMasterTagSettingModalComponent;
    @ViewChild('viewBusinessMasterTagSettingModal', { static: true })
    viewBusinessMasterTagSettingModal: ViewBusinessMasterTagSettingModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxDisplaySequenceFilter: number;
    maxDisplaySequenceFilterEmpty: number;
    minDisplaySequenceFilter: number;
    minDisplaySequenceFilterEmpty: number;
    displayPublicFilter = -1;
    customTagTitleFilter = '';
    customTagChatQuestionFilter = '';
    answerTypeIdFilter = -1;
    masterTagCategoryNameFilter = '';
    masterTagNameFilter = '';

    answerType = AnswerType;

    constructor(
        injector: Injector,
        private _businessMasterTagSettingsServiceProxy: BusinessMasterTagSettingsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getBusinessMasterTagSettings(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._businessMasterTagSettingsServiceProxy
            .getAll(
                this.filterText,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.displayPublicFilter,
                this.customTagTitleFilter,
                this.customTagChatQuestionFilter,
                this.answerTypeIdFilter,
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

    createBusinessMasterTagSetting(): void {
        this.createOrEditBusinessMasterTagSettingModal.show();
    }

    deleteBusinessMasterTagSetting(businessMasterTagSetting: BusinessMasterTagSettingDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._businessMasterTagSettingsServiceProxy.delete(businessMasterTagSetting.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._businessMasterTagSettingsServiceProxy
            .getBusinessMasterTagSettingsToExcel(
                this.filterText,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.displayPublicFilter,
                this.customTagTitleFilter,
                this.customTagChatQuestionFilter,
                this.answerTypeIdFilter,
                this.masterTagCategoryNameFilter,
                this.masterTagNameFilter
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
        this.customTagTitleFilter = '';
        this.customTagChatQuestionFilter = '';
        this.answerTypeIdFilter = -1;
        this.masterTagCategoryNameFilter = '';
        this.masterTagNameFilter = '';

        this.getBusinessMasterTagSettings();
    }
}
