import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { ContactCertificationLicensesServiceProxy, ContactCertificationLicenseDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditContactCertificationLicenseModalComponent } from './create-or-edit-contactCertificationLicense-modal.component';

import { ViewContactCertificationLicenseModalComponent } from './view-contactCertificationLicense-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './contactCertificationLicenses.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ContactCertificationLicensesComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditContactCertificationLicenseModal', { static: true }) createOrEditContactCertificationLicenseModal: CreateOrEditContactCertificationLicenseModalComponent;
    @ViewChild('viewContactCertificationLicenseModal', { static: true }) viewContactCertificationLicenseModal: ViewContactCertificationLicenseModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    maxFromDateFilter : DateTime;
		minFromDateFilter : DateTime;
    maxToDateFilter : DateTime;
		minToDateFilter : DateTime;
    instituteNameFilter = '';
    addressFilter = '';
    descriptionFilter = '';
    licenseNumberFilter = '';
    maxIssueDateFilter : DateTime;
		minIssueDateFilter : DateTime;
    maxExpirationDateFilter : DateTime;
		minExpirationDateFilter : DateTime;
    verifiedFilter = -1;
    thumbsUpDownFilter = -1;
    internalNotesFilter = '';
    maxDisplaySequenceFilter : number;
		maxDisplaySequenceFilterEmpty : number;
		minDisplaySequenceFilter : number;
		minDisplaySequenceFilterEmpty : number;
        contactFullNameFilter = '';
        contactDocumentDocumentTitleFilter = '';
        employeeNameFilter = '';
        businessNameFilter = '';






    constructor(
        injector: Injector,
        private _contactCertificationLicensesServiceProxy: ContactCertificationLicensesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getContactCertificationLicenses(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._contactCertificationLicensesServiceProxy.getAll(
            this.filterText,
            this.nameFilter,
            this.maxFromDateFilter === undefined ? this.maxFromDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxFromDateFilter),
            this.minFromDateFilter === undefined ? this.minFromDateFilter : this._dateTimeService.getStartOfDayForDate(this.minFromDateFilter),
            this.maxToDateFilter === undefined ? this.maxToDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxToDateFilter),
            this.minToDateFilter === undefined ? this.minToDateFilter : this._dateTimeService.getStartOfDayForDate(this.minToDateFilter),
            this.instituteNameFilter,
            this.addressFilter,
            this.descriptionFilter,
            this.licenseNumberFilter,
            this.maxIssueDateFilter === undefined ? this.maxIssueDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxIssueDateFilter),
            this.minIssueDateFilter === undefined ? this.minIssueDateFilter : this._dateTimeService.getStartOfDayForDate(this.minIssueDateFilter),
            this.maxExpirationDateFilter === undefined ? this.maxExpirationDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxExpirationDateFilter),
            this.minExpirationDateFilter === undefined ? this.minExpirationDateFilter : this._dateTimeService.getStartOfDayForDate(this.minExpirationDateFilter),
            this.verifiedFilter,
            this.thumbsUpDownFilter,
            this.internalNotesFilter,
            this.maxDisplaySequenceFilter == null ? this.maxDisplaySequenceFilterEmpty: this.maxDisplaySequenceFilter,
            this.minDisplaySequenceFilter == null ? this.minDisplaySequenceFilterEmpty: this.minDisplaySequenceFilter,
            this.contactFullNameFilter,
            this.contactDocumentDocumentTitleFilter,
            this.employeeNameFilter,
            this.businessNameFilter,
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

    createContactCertificationLicense(): void {
        this.createOrEditContactCertificationLicenseModal.show();        
    }


    deleteContactCertificationLicense(contactCertificationLicense: ContactCertificationLicenseDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._contactCertificationLicensesServiceProxy.delete(contactCertificationLicense.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._contactCertificationLicensesServiceProxy.getContactCertificationLicensesToExcel(
        this.filterText,
            this.nameFilter,
            this.maxFromDateFilter === undefined ? this.maxFromDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxFromDateFilter),
            this.minFromDateFilter === undefined ? this.minFromDateFilter : this._dateTimeService.getStartOfDayForDate(this.minFromDateFilter),
            this.maxToDateFilter === undefined ? this.maxToDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxToDateFilter),
            this.minToDateFilter === undefined ? this.minToDateFilter : this._dateTimeService.getStartOfDayForDate(this.minToDateFilter),
            this.instituteNameFilter,
            this.addressFilter,
            this.descriptionFilter,
            this.licenseNumberFilter,
            this.maxIssueDateFilter === undefined ? this.maxIssueDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxIssueDateFilter),
            this.minIssueDateFilter === undefined ? this.minIssueDateFilter : this._dateTimeService.getStartOfDayForDate(this.minIssueDateFilter),
            this.maxExpirationDateFilter === undefined ? this.maxExpirationDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxExpirationDateFilter),
            this.minExpirationDateFilter === undefined ? this.minExpirationDateFilter : this._dateTimeService.getStartOfDayForDate(this.minExpirationDateFilter),
            this.verifiedFilter,
            this.thumbsUpDownFilter,
            this.internalNotesFilter,
            this.maxDisplaySequenceFilter == null ? this.maxDisplaySequenceFilterEmpty: this.maxDisplaySequenceFilter,
            this.minDisplaySequenceFilter == null ? this.minDisplaySequenceFilterEmpty: this.minDisplaySequenceFilter,
            this.contactFullNameFilter,
            this.contactDocumentDocumentTitleFilter,
            this.employeeNameFilter,
            this.businessNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.nameFilter = '';
    this.maxFromDateFilter = undefined;
		this.minFromDateFilter = undefined;
    this.maxToDateFilter = undefined;
		this.minToDateFilter = undefined;
    this.instituteNameFilter = '';
    this.addressFilter = '';
    this.descriptionFilter = '';
    this.licenseNumberFilter = '';
    this.maxIssueDateFilter = undefined;
		this.minIssueDateFilter = undefined;
    this.maxExpirationDateFilter = undefined;
		this.minExpirationDateFilter = undefined;
    this.verifiedFilter = -1;
    this.thumbsUpDownFilter = -1;
    this.internalNotesFilter = '';
    this.maxDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
		this.minDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
		this.contactFullNameFilter = '';
							this.contactDocumentDocumentTitleFilter = '';
							this.employeeNameFilter = '';
							this.businessNameFilter = '';
					
        this.getContactCertificationLicenses();
    }
}
