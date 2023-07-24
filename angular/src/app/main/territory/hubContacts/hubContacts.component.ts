import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HubContactsServiceProxy, HubContactDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditHubContactModalComponent } from './create-or-edit-hubContact-modal.component';

import { ViewHubContactModalComponent } from './view-hubContact-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'app-hubContacts',
    templateUrl: './hubContacts.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class HubContactsComponent extends AppComponentBase {
    @ViewChild('createOrEditHubContactModal', { static: true })
    createOrEditHubContactModal: CreateOrEditHubContactModalComponent;
    @ViewChild('viewHubContactModal', { static: true }) viewHubContactModal: ViewHubContactModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxDisplayScoreFilter: number;
    maxDisplayScoreFilterEmpty: number;
    minDisplayScoreFilter: number;
    minDisplayScoreFilterEmpty: number;
    hubNameFilter = '';
    contactFullNameFilter = '';

    constructor(
        injector: Injector,
        private _hubContactsServiceProxy: HubContactsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getHubContacts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._hubContactsServiceProxy
            .getAll(
                this.filterText,
                this.maxDisplayScoreFilter == null ? this.maxDisplayScoreFilterEmpty : this.maxDisplayScoreFilter,
                this.minDisplayScoreFilter == null ? this.minDisplayScoreFilterEmpty : this.minDisplayScoreFilter,
                this.hubNameFilter,
                this.contactFullNameFilter,
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

    createHubContact(): void {
        this.createOrEditHubContactModal.show();
    }

    deleteHubContact(hubContact: HubContactDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._hubContactsServiceProxy.delete(hubContact.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._hubContactsServiceProxy
            .getHubContactsToExcel(
                this.filterText,
                this.maxDisplayScoreFilter == null ? this.maxDisplayScoreFilterEmpty : this.maxDisplayScoreFilter,
                this.minDisplayScoreFilter == null ? this.minDisplayScoreFilterEmpty : this.minDisplayScoreFilter,
                this.hubNameFilter,
                this.contactFullNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxDisplayScoreFilter = this.maxDisplayScoreFilterEmpty;
        this.minDisplayScoreFilter = this.maxDisplayScoreFilterEmpty;
        this.hubNameFilter = '';
        this.contactFullNameFilter = '';

        this.getHubContacts();
    }
}
