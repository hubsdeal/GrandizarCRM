import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ContactsServiceProxy, ContactDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditContactModalComponent } from './create-or-edit-contact-modal.component';

import { ViewContactModalComponent } from './view-contact-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'app-contacts',
    templateUrl: './contacts.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ContactsComponent extends AppComponentBase {
    @ViewChild('createOrEditContactModal', { static: true })
    createOrEditContactModal: CreateOrEditContactModalComponent;
    @ViewChild('viewContactModal', { static: true }) viewContactModal: ViewContactModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    fullNameFilter = '';
    firstNameFilter = '';
    lastNameFilter = '';
    fullAddressFilter = '';
    addressFilter = '';
    zipCodeFilter = '';
    cityFilter = '';
    maxDateOfBirthFilter: DateTime;
    minDateOfBirthFilter: DateTime;
    countryCodeFilter = '';
    personalEmailFilter = '';
    businessEmailFilter = '';
    jobTitleFilter = '';
    companyNameFilter = '';
    profileFilter = '';
    aiDataTagFilter = '';
    facebookFilter = '';
    linkedInFilter = '';
    referredFilter = -1;
    verifiedFilter = -1;
    maxScoreFilter: number;
    maxScoreFilterEmpty: number;
    minScoreFilter: number;
    minScoreFilterEmpty: number;
    userNameFilter = '';
    countryNameFilter = '';
    stateNameFilter = '';
    membershipTypeNameFilter = '';

    constructor(
        injector: Injector,
        private _contactsServiceProxy: ContactsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getContacts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._contactsServiceProxy
            .getAll(
                this.filterText,
                this.fullNameFilter,
                this.firstNameFilter,
                this.lastNameFilter,
                this.fullAddressFilter,
                this.addressFilter,
                this.zipCodeFilter,
                this.cityFilter,
                this.maxDateOfBirthFilter === undefined
                    ? this.maxDateOfBirthFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxDateOfBirthFilter),
                this.minDateOfBirthFilter === undefined
                    ? this.minDateOfBirthFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minDateOfBirthFilter),
                this.countryCodeFilter,
                this.personalEmailFilter,
                this.businessEmailFilter,
                this.jobTitleFilter,
                this.companyNameFilter,
                this.profileFilter,
                this.aiDataTagFilter,
                this.facebookFilter,
                this.linkedInFilter,
                this.referredFilter,
                this.verifiedFilter,
                this.maxScoreFilter == null ? this.maxScoreFilterEmpty : this.maxScoreFilter,
                this.minScoreFilter == null ? this.minScoreFilterEmpty : this.minScoreFilter,
                this.userNameFilter,
                this.countryNameFilter,
                this.stateNameFilter,
                this.membershipTypeNameFilter,
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

    createContact(): void {
        this.createOrEditContactModal.show();
    }

    deleteContact(contact: ContactDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._contactsServiceProxy.delete(contact.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._contactsServiceProxy
            .getContactsToExcel(
                this.filterText,
                this.fullNameFilter,
                this.firstNameFilter,
                this.lastNameFilter,
                this.fullAddressFilter,
                this.addressFilter,
                this.zipCodeFilter,
                this.cityFilter,
                this.maxDateOfBirthFilter === undefined
                    ? this.maxDateOfBirthFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxDateOfBirthFilter),
                this.minDateOfBirthFilter === undefined
                    ? this.minDateOfBirthFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minDateOfBirthFilter),
                this.countryCodeFilter,
                this.personalEmailFilter,
                this.businessEmailFilter,
                this.jobTitleFilter,
                this.companyNameFilter,
                this.profileFilter,
                this.aiDataTagFilter,
                this.facebookFilter,
                this.linkedInFilter,
                this.referredFilter,
                this.verifiedFilter,
                this.maxScoreFilter == null ? this.maxScoreFilterEmpty : this.maxScoreFilter,
                this.minScoreFilter == null ? this.minScoreFilterEmpty : this.minScoreFilter,
                this.userNameFilter,
                this.countryNameFilter,
                this.stateNameFilter,
                this.membershipTypeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.fullNameFilter = '';
        this.firstNameFilter = '';
        this.lastNameFilter = '';
        this.fullAddressFilter = '';
        this.addressFilter = '';
        this.zipCodeFilter = '';
        this.cityFilter = '';
        this.maxDateOfBirthFilter = undefined;
        this.minDateOfBirthFilter = undefined;
        this.countryCodeFilter = '';
        this.personalEmailFilter = '';
        this.businessEmailFilter = '';
        this.jobTitleFilter = '';
        this.companyNameFilter = '';
        this.profileFilter = '';
        this.aiDataTagFilter = '';
        this.facebookFilter = '';
        this.linkedInFilter = '';
        this.referredFilter = -1;
        this.verifiedFilter = -1;
        this.maxScoreFilter = this.maxScoreFilterEmpty;
        this.minScoreFilter = this.maxScoreFilterEmpty;
        this.userNameFilter = '';
        this.countryNameFilter = '';
        this.stateNameFilter = '';
        this.membershipTypeNameFilter = '';

        this.getContacts();
    }
}
