import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { ContactReferralContactsServiceProxy, ContactReferralContactDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditContactReferralContactModalComponent } from './create-or-edit-contactReferralContact-modal.component';

import { ViewContactReferralContactModalComponent } from './view-contactReferralContact-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './contactReferralContacts.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ContactReferralContactsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditContactReferralContactModal', { static: true }) createOrEditContactReferralContactModal: CreateOrEditContactReferralContactModalComponent;
    @ViewChild('viewContactReferralContactModal', { static: true }) viewContactReferralContactModal: ViewContactReferralContactModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    firstNameFilter = '';
    lastNameFilter = '';
    emailFilter = '';
    mobileFilter = '';
    facebookLinkFilter = '';
    referredFilter = -1;
    uniqueReferredFilter = -1;
    maxReferredDateTimeFilter : DateTime;
		minReferredDateTimeFilter : DateTime;
        contactFullNameFilter = '';
        contactFullName2Filter = '';
        contactFullName3Filter = '';






    constructor(
        injector: Injector,
        private _contactReferralContactsServiceProxy: ContactReferralContactsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getContactReferralContacts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._contactReferralContactsServiceProxy.getAll(
            this.filterText,
            this.firstNameFilter,
            this.lastNameFilter,
            this.emailFilter,
            this.mobileFilter,
            this.facebookLinkFilter,
            this.referredFilter,
            this.uniqueReferredFilter,
            this.maxReferredDateTimeFilter === undefined ? this.maxReferredDateTimeFilter : this._dateTimeService.getEndOfDayForDate(this.maxReferredDateTimeFilter),
            this.minReferredDateTimeFilter === undefined ? this.minReferredDateTimeFilter : this._dateTimeService.getStartOfDayForDate(this.minReferredDateTimeFilter),
            this.contactFullNameFilter,
            this.contactFullName2Filter,
            this.contactFullName3Filter,
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

    createContactReferralContact(): void {
        this.createOrEditContactReferralContactModal.show();        
    }


    deleteContactReferralContact(contactReferralContact: ContactReferralContactDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._contactReferralContactsServiceProxy.delete(contactReferralContact.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._contactReferralContactsServiceProxy.getContactReferralContactsToExcel(
        this.filterText,
            this.firstNameFilter,
            this.lastNameFilter,
            this.emailFilter,
            this.mobileFilter,
            this.facebookLinkFilter,
            this.referredFilter,
            this.uniqueReferredFilter,
            this.maxReferredDateTimeFilter === undefined ? this.maxReferredDateTimeFilter : this._dateTimeService.getEndOfDayForDate(this.maxReferredDateTimeFilter),
            this.minReferredDateTimeFilter === undefined ? this.minReferredDateTimeFilter : this._dateTimeService.getStartOfDayForDate(this.minReferredDateTimeFilter),
            this.contactFullNameFilter,
            this.contactFullName2Filter,
            this.contactFullName3Filter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.firstNameFilter = '';
    this.lastNameFilter = '';
    this.emailFilter = '';
    this.mobileFilter = '';
    this.facebookLinkFilter = '';
    this.referredFilter = -1;
    this.uniqueReferredFilter = -1;
    this.maxReferredDateTimeFilter = undefined;
		this.minReferredDateTimeFilter = undefined;
		this.contactFullNameFilter = '';
							this.contactFullName2Filter = '';
							this.contactFullName3Filter = '';
					
        this.getContactReferralContacts();
    }
}
