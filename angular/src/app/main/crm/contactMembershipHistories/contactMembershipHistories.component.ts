import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { ContactMembershipHistoriesServiceProxy, ContactMembershipHistoryDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditContactMembershipHistoryModalComponent } from './create-or-edit-contactMembershipHistory-modal.component';

import { ViewContactMembershipHistoryModalComponent } from './view-contactMembershipHistory-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './contactMembershipHistories.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ContactMembershipHistoriesComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditContactMembershipHistoryModal', { static: true }) createOrEditContactMembershipHistoryModal: CreateOrEditContactMembershipHistoryModalComponent;
    @ViewChild('viewContactMembershipHistoryModal', { static: true }) viewContactMembershipHistoryModal: ViewContactMembershipHistoryModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxStartDateFilter : DateTime;
		minStartDateFilter : DateTime;
    maxExpirationDateFilter : DateTime;
		minExpirationDateFilter : DateTime;
    activeFilter = -1;
        contactFullNameFilter = '';
        membershipTypeNameFilter = '';
        productNameFilter = '';






    constructor(
        injector: Injector,
        private _contactMembershipHistoriesServiceProxy: ContactMembershipHistoriesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getContactMembershipHistories(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._contactMembershipHistoriesServiceProxy.getAll(
            this.filterText,
            this.maxStartDateFilter === undefined ? this.maxStartDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
            this.minStartDateFilter === undefined ? this.minStartDateFilter : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
            this.maxExpirationDateFilter === undefined ? this.maxExpirationDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxExpirationDateFilter),
            this.minExpirationDateFilter === undefined ? this.minExpirationDateFilter : this._dateTimeService.getStartOfDayForDate(this.minExpirationDateFilter),
            this.activeFilter,
            this.contactFullNameFilter,
            this.membershipTypeNameFilter,
            this.productNameFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createContactMembershipHistory(): void {
        this.createOrEditContactMembershipHistoryModal.show();        
    }


    deleteContactMembershipHistory(contactMembershipHistory: ContactMembershipHistoryDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._contactMembershipHistoriesServiceProxy.delete(contactMembershipHistory.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._contactMembershipHistoriesServiceProxy.getContactMembershipHistoriesToExcel(
        this.filterText,
            this.maxStartDateFilter === undefined ? this.maxStartDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
            this.minStartDateFilter === undefined ? this.minStartDateFilter : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
            this.maxExpirationDateFilter === undefined ? this.maxExpirationDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxExpirationDateFilter),
            this.minExpirationDateFilter === undefined ? this.minExpirationDateFilter : this._dateTimeService.getStartOfDayForDate(this.minExpirationDateFilter),
            this.activeFilter,
            this.contactFullNameFilter,
            this.membershipTypeNameFilter,
            this.productNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.maxStartDateFilter = undefined;
		this.minStartDateFilter = undefined;
    this.maxExpirationDateFilter = undefined;
		this.minExpirationDateFilter = undefined;
    this.activeFilter = -1;
		this.contactFullNameFilter = '';
							this.membershipTypeNameFilter = '';
							this.productNameFilter = '';
					
        this.getContactMembershipHistories();
    }
}
