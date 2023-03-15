import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SmsTemplatesServiceProxy, SmsTemplateDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditSmsTemplateModalComponent } from './create-or-edit-smsTemplate-modal.component';

import { ViewSmsTemplateModalComponent } from './view-smsTemplate-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './smsTemplates.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class SmsTemplatesComponent extends AppComponentBase {
    @ViewChild('createOrEditSmsTemplateModal', { static: true })
    createOrEditSmsTemplateModal: CreateOrEditSmsTemplateModalComponent;
    @ViewChild('viewSmsTemplateModal', { static: true }) viewSmsTemplateModal: ViewSmsTemplateModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    titleFilter = '';
    contentFilter = '';
    publishedFilter = -1;

    constructor(
        injector: Injector,
        private _smsTemplatesServiceProxy: SmsTemplatesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getSmsTemplates(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._smsTemplatesServiceProxy
            .getAll(
                this.filterText,
                this.titleFilter,
                this.contentFilter,
                this.publishedFilter,
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

    createSmsTemplate(): void {
        this.createOrEditSmsTemplateModal.show();
    }

    deleteSmsTemplate(smsTemplate: SmsTemplateDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._smsTemplatesServiceProxy.delete(smsTemplate.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._smsTemplatesServiceProxy
            .getSmsTemplatesToExcel(this.filterText, this.titleFilter, this.contentFilter, this.publishedFilter)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.titleFilter = '';
        this.contentFilter = '';
        this.publishedFilter = -1;

        this.getSmsTemplates();
    }
}
