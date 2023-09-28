import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { ContactExperiencesServiceProxy, ContactExperienceDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditContactExperienceModalComponent } from './create-or-edit-contactExperience-modal.component';

import { ViewContactExperienceModalComponent } from './view-contactExperience-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './contactExperiences.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ContactExperiencesComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditContactExperienceModal', { static: true }) createOrEditContactExperienceModal: CreateOrEditContactExperienceModalComponent;
    @ViewChild('viewContactExperienceModal', { static: true }) viewContactExperienceModal: ViewContactExperienceModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    jobTitleFilter = '';
    currentFilter = -1;
    employerNameFilter = '';
    employerWebsiteFilter = '';
    locationFilter = '';
    maxStartDateFilter : DateTime;
		minStartDateFilter : DateTime;
    maxEndDateFilter : DateTime;
		minEndDateFilter : DateTime;
    jobDescriptionFilter = '';
    verifiedFilter = -1;
    thumbsUpDownFilter = -1;
    internalNotesFilter = '';
    maxGrossSalaryFilter : number;
		maxGrossSalaryFilterEmpty : number;
		minGrossSalaryFilter : number;
		minGrossSalaryFilterEmpty : number;
    maxDisplaySequenceFilter : number;
		maxDisplaySequenceFilterEmpty : number;
		minDisplaySequenceFilter : number;
		minDisplaySequenceFilterEmpty : number;
        contactFullNameFilter = '';
        employeeNameFilter = '';
        businessNameFilter = '';
        currencyNameFilter = '';






    constructor(
        injector: Injector,
        private _contactExperiencesServiceProxy: ContactExperiencesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getContactExperiences(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._contactExperiencesServiceProxy.getAll(
            this.filterText,
            this.jobTitleFilter,
            this.currentFilter,
            this.employerNameFilter,
            this.employerWebsiteFilter,
            this.locationFilter,
            this.maxStartDateFilter === undefined ? this.maxStartDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
            this.minStartDateFilter === undefined ? this.minStartDateFilter : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
            this.maxEndDateFilter === undefined ? this.maxEndDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
            this.minEndDateFilter === undefined ? this.minEndDateFilter : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
            this.jobDescriptionFilter,
            this.verifiedFilter,
            this.thumbsUpDownFilter,
            this.internalNotesFilter,
            this.maxGrossSalaryFilter == null ? this.maxGrossSalaryFilterEmpty: this.maxGrossSalaryFilter,
            this.minGrossSalaryFilter == null ? this.minGrossSalaryFilterEmpty: this.minGrossSalaryFilter,
            this.maxDisplaySequenceFilter == null ? this.maxDisplaySequenceFilterEmpty: this.maxDisplaySequenceFilter,
            this.minDisplaySequenceFilter == null ? this.minDisplaySequenceFilterEmpty: this.minDisplaySequenceFilter,
            this.contactFullNameFilter,
            this.employeeNameFilter,
            this.businessNameFilter,
            this.currencyNameFilter,
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

    createContactExperience(): void {
        this.createOrEditContactExperienceModal.show();        
    }


    deleteContactExperience(contactExperience: ContactExperienceDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._contactExperiencesServiceProxy.delete(contactExperience.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._contactExperiencesServiceProxy.getContactExperiencesToExcel(
        this.filterText,
            this.jobTitleFilter,
            this.currentFilter,
            this.employerNameFilter,
            this.employerWebsiteFilter,
            this.locationFilter,
            this.maxStartDateFilter === undefined ? this.maxStartDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
            this.minStartDateFilter === undefined ? this.minStartDateFilter : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
            this.maxEndDateFilter === undefined ? this.maxEndDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
            this.minEndDateFilter === undefined ? this.minEndDateFilter : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
            this.jobDescriptionFilter,
            this.verifiedFilter,
            this.thumbsUpDownFilter,
            this.internalNotesFilter,
            this.maxGrossSalaryFilter == null ? this.maxGrossSalaryFilterEmpty: this.maxGrossSalaryFilter,
            this.minGrossSalaryFilter == null ? this.minGrossSalaryFilterEmpty: this.minGrossSalaryFilter,
            this.maxDisplaySequenceFilter == null ? this.maxDisplaySequenceFilterEmpty: this.maxDisplaySequenceFilter,
            this.minDisplaySequenceFilter == null ? this.minDisplaySequenceFilterEmpty: this.minDisplaySequenceFilter,
            this.contactFullNameFilter,
            this.employeeNameFilter,
            this.businessNameFilter,
            this.currencyNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.jobTitleFilter = '';
    this.currentFilter = -1;
    this.employerNameFilter = '';
    this.employerWebsiteFilter = '';
    this.locationFilter = '';
    this.maxStartDateFilter = undefined;
		this.minStartDateFilter = undefined;
    this.maxEndDateFilter = undefined;
		this.minEndDateFilter = undefined;
    this.jobDescriptionFilter = '';
    this.verifiedFilter = -1;
    this.thumbsUpDownFilter = -1;
    this.internalNotesFilter = '';
    this.maxGrossSalaryFilter = this.maxGrossSalaryFilterEmpty;
		this.minGrossSalaryFilter = this.maxGrossSalaryFilterEmpty;
    this.maxDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
		this.minDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
		this.contactFullNameFilter = '';
							this.employeeNameFilter = '';
							this.businessNameFilter = '';
							this.currencyNameFilter = '';
					
        this.getContactExperiences();
    }
}
