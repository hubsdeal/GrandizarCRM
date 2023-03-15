import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmailTemplatesServiceProxy, EmailTemplateDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditEmailTemplateModalComponent } from './create-or-edit-emailTemplate-modal.component';

import { ViewEmailTemplateModalComponent } from './view-emailTemplate-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './emailTemplates.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class EmailTemplatesComponent extends AppComponentBase {
    @ViewChild('createOrEditEmailTemplateModal', { static: true })
    createOrEditEmailTemplateModal: CreateOrEditEmailTemplateModalComponent;
    @ViewChild('viewEmailTemplateModal', { static: true }) viewEmailTemplateModal: ViewEmailTemplateModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    subjectFilter = '';
    contentFilter = '';
    publishedFilter = -1;

    constructor(
        injector: Injector,
        private _emailTemplatesServiceProxy: EmailTemplatesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getEmailTemplates(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._emailTemplatesServiceProxy
            .getAll(
                this.filterText,
                this.subjectFilter,
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

    createEmailTemplate(): void {
        this.createOrEditEmailTemplateModal.show();
    }

    deleteEmailTemplate(emailTemplate: EmailTemplateDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._emailTemplatesServiceProxy.delete(emailTemplate.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._emailTemplatesServiceProxy
            .getEmailTemplatesToExcel(this.filterText, this.subjectFilter, this.contentFilter, this.publishedFilter)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.subjectFilter = '';
        this.contentFilter = '';
        this.publishedFilter = -1;

        this.getEmailTemplates();
    }
}
