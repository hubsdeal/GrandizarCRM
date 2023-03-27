import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LeadContactsServiceProxy, LeadContactDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditLeadContactModalComponent } from './create-or-edit-leadContact-modal.component';

import { ViewLeadContactModalComponent } from './view-leadContact-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './leadContacts.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class LeadContactsComponent extends AppComponentBase {
    @ViewChild('createOrEditLeadContactModal', { static: true })
    createOrEditLeadContactModal: CreateOrEditLeadContactModalComponent;
    @ViewChild('viewLeadContactModal', { static: true }) viewLeadContactModal: ViewLeadContactModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    notesFilter = '';
    maxInfluenceScoreFilter: number;
    maxInfluenceScoreFilterEmpty: number;
    minInfluenceScoreFilter: number;
    minInfluenceScoreFilterEmpty: number;
    leadTitleFilter = '';
    contactFullNameFilter = '';

    constructor(
        injector: Injector,
        private _leadContactsServiceProxy: LeadContactsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getLeadContacts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._leadContactsServiceProxy
            .getAll(
                this.filterText,
                this.notesFilter,
                this.maxInfluenceScoreFilter == null ? this.maxInfluenceScoreFilterEmpty : this.maxInfluenceScoreFilter,
                this.minInfluenceScoreFilter == null ? this.minInfluenceScoreFilterEmpty : this.minInfluenceScoreFilter,
                this.leadTitleFilter,
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

    createLeadContact(): void {
        this.createOrEditLeadContactModal.show();
    }

    deleteLeadContact(leadContact: LeadContactDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._leadContactsServiceProxy.delete(leadContact.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._leadContactsServiceProxy
            .getLeadContactsToExcel(
                this.filterText,
                this.notesFilter,
                this.maxInfluenceScoreFilter == null ? this.maxInfluenceScoreFilterEmpty : this.maxInfluenceScoreFilter,
                this.minInfluenceScoreFilter == null ? this.minInfluenceScoreFilterEmpty : this.minInfluenceScoreFilter,
                this.leadTitleFilter,
                this.contactFullNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.notesFilter = '';
        this.maxInfluenceScoreFilter = this.maxInfluenceScoreFilterEmpty;
        this.minInfluenceScoreFilter = this.maxInfluenceScoreFilterEmpty;
        this.leadTitleFilter = '';
        this.contactFullNameFilter = '';

        this.getLeadContacts();
    }
}
