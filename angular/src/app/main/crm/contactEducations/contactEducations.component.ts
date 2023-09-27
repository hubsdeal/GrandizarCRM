import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { ContactEducationsServiceProxy, ContactEducationDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditContactEducationModalComponent } from './create-or-edit-contactEducation-modal.component';

import { ViewContactEducationModalComponent } from './view-contactEducation-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './contactEducations.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ContactEducationsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditContactEducationModal', { static: true }) createOrEditContactEducationModal: CreateOrEditContactEducationModalComponent;
    @ViewChild('viewContactEducationModal', { static: true }) viewContactEducationModal: ViewContactEducationModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    highestDegreeFilter = -1;
    instituteNameFilter = '';
    majorOrMinorDegreeSubjectFilter = '';
    maxStartDateFilter : DateTime;
		minStartDateFilter : DateTime;
    maxEndDateFilter : DateTime;
		minEndDateFilter : DateTime;
    maxPassingGpaFilter : number;
		maxPassingGpaFilterEmpty : number;
		minPassingGpaFilter : number;
		minPassingGpaFilterEmpty : number;
    locationFilter = '';
    verifiedFilter = -1;
    thumbsUpDownFilter = -1;
    internalNotesFilter = '';
    maxDisplaySequenceFilter : number;
		maxDisplaySequenceFilterEmpty : number;
		minDisplaySequenceFilter : number;
		minDisplaySequenceFilterEmpty : number;
        contactFullNameFilter = '';
        employeeNameFilter = '';
        businessNameFilter = '';
        contactDocumentDocumentTitleFilter = '';






    constructor(
        injector: Injector,
        private _contactEducationsServiceProxy: ContactEducationsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getContactEducations(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._contactEducationsServiceProxy.getAll(
            this.filterText,
            this.nameFilter,
            this.highestDegreeFilter,
            this.instituteNameFilter,
            this.majorOrMinorDegreeSubjectFilter,
            this.maxStartDateFilter === undefined ? this.maxStartDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
            this.minStartDateFilter === undefined ? this.minStartDateFilter : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
            this.maxEndDateFilter === undefined ? this.maxEndDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
            this.minEndDateFilter === undefined ? this.minEndDateFilter : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
            this.maxPassingGpaFilter == null ? this.maxPassingGpaFilterEmpty: this.maxPassingGpaFilter,
            this.minPassingGpaFilter == null ? this.minPassingGpaFilterEmpty: this.minPassingGpaFilter,
            this.locationFilter,
            this.verifiedFilter,
            this.thumbsUpDownFilter,
            this.internalNotesFilter,
            this.maxDisplaySequenceFilter == null ? this.maxDisplaySequenceFilterEmpty: this.maxDisplaySequenceFilter,
            this.minDisplaySequenceFilter == null ? this.minDisplaySequenceFilterEmpty: this.minDisplaySequenceFilter,
            this.contactFullNameFilter,
            this.employeeNameFilter,
            this.businessNameFilter,
            this.contactDocumentDocumentTitleFilter,
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

    createContactEducation(): void {
        this.createOrEditContactEducationModal.show();        
    }


    deleteContactEducation(contactEducation: ContactEducationDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._contactEducationsServiceProxy.delete(contactEducation.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._contactEducationsServiceProxy.getContactEducationsToExcel(
        this.filterText,
            this.nameFilter,
            this.highestDegreeFilter,
            this.instituteNameFilter,
            this.majorOrMinorDegreeSubjectFilter,
            this.maxStartDateFilter === undefined ? this.maxStartDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
            this.minStartDateFilter === undefined ? this.minStartDateFilter : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
            this.maxEndDateFilter === undefined ? this.maxEndDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
            this.minEndDateFilter === undefined ? this.minEndDateFilter : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
            this.maxPassingGpaFilter == null ? this.maxPassingGpaFilterEmpty: this.maxPassingGpaFilter,
            this.minPassingGpaFilter == null ? this.minPassingGpaFilterEmpty: this.minPassingGpaFilter,
            this.locationFilter,
            this.verifiedFilter,
            this.thumbsUpDownFilter,
            this.internalNotesFilter,
            this.maxDisplaySequenceFilter == null ? this.maxDisplaySequenceFilterEmpty: this.maxDisplaySequenceFilter,
            this.minDisplaySequenceFilter == null ? this.minDisplaySequenceFilterEmpty: this.minDisplaySequenceFilter,
            this.contactFullNameFilter,
            this.employeeNameFilter,
            this.businessNameFilter,
            this.contactDocumentDocumentTitleFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.nameFilter = '';
    this.highestDegreeFilter = -1;
    this.instituteNameFilter = '';
    this.majorOrMinorDegreeSubjectFilter = '';
    this.maxStartDateFilter = undefined;
		this.minStartDateFilter = undefined;
    this.maxEndDateFilter = undefined;
		this.minEndDateFilter = undefined;
    this.maxPassingGpaFilter = this.maxPassingGpaFilterEmpty;
		this.minPassingGpaFilter = this.maxPassingGpaFilterEmpty;
    this.locationFilter = '';
    this.verifiedFilter = -1;
    this.thumbsUpDownFilter = -1;
    this.internalNotesFilter = '';
    this.maxDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
		this.minDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
		this.contactFullNameFilter = '';
							this.employeeNameFilter = '';
							this.businessNameFilter = '';
							this.contactDocumentDocumentTitleFilter = '';
					
        this.getContactEducations();
    }
}
