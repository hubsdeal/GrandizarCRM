import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { JobsServiceProxy, CreateOrEditJobDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { JobMasterTagCategoryLookupTableModalComponent } from './job-masterTagCategory-lookup-table-modal.component';
import { JobMasterTagLookupTableModalComponent } from './job-masterTag-lookup-table-modal.component';
import { JobProductCategoryLookupTableModalComponent } from './job-productCategory-lookup-table-modal.component';
import { JobCurrencyLookupTableModalComponent } from './job-currency-lookup-table-modal.component';
import { JobBusinessLookupTableModalComponent } from './job-business-lookup-table-modal.component';
import { JobCountryLookupTableModalComponent } from './job-country-lookup-table-modal.component';
import { JobStateLookupTableModalComponent } from './job-state-lookup-table-modal.component';
import { JobCityLookupTableModalComponent } from './job-city-lookup-table-modal.component';
import { JobJobStatusTypeLookupTableModalComponent } from './job-jobStatusType-lookup-table-modal.component';
import { JobStoreLookupTableModalComponent } from './job-store-lookup-table-modal.component';

@Component({
    selector: 'createOrEditJobModal',
    templateUrl: './create-or-edit-job-modal.component.html',
})
export class CreateOrEditJobModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('jobMasterTagCategoryLookupTableModal', { static: true })
    jobMasterTagCategoryLookupTableModal: JobMasterTagCategoryLookupTableModalComponent;
    @ViewChild('jobMasterTagLookupTableModal', { static: true })
    jobMasterTagLookupTableModal: JobMasterTagLookupTableModalComponent;
    @ViewChild('jobProductCategoryLookupTableModal', { static: true })
    jobProductCategoryLookupTableModal: JobProductCategoryLookupTableModalComponent;
    @ViewChild('jobCurrencyLookupTableModal', { static: true })
    jobCurrencyLookupTableModal: JobCurrencyLookupTableModalComponent;
    @ViewChild('jobBusinessLookupTableModal', { static: true })
    jobBusinessLookupTableModal: JobBusinessLookupTableModalComponent;
    @ViewChild('jobCountryLookupTableModal', { static: true })
    jobCountryLookupTableModal: JobCountryLookupTableModalComponent;
    @ViewChild('jobStateLookupTableModal', { static: true })
    jobStateLookupTableModal: JobStateLookupTableModalComponent;
    @ViewChild('jobCityLookupTableModal', { static: true }) jobCityLookupTableModal: JobCityLookupTableModalComponent;
    @ViewChild('jobJobStatusTypeLookupTableModal', { static: true })
    jobJobStatusTypeLookupTableModal: JobJobStatusTypeLookupTableModalComponent;
    @ViewChild('jobStoreLookupTableModal', { static: true })
    jobStoreLookupTableModal: JobStoreLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    job: CreateOrEditJobDto = new CreateOrEditJobDto();

    masterTagCategoryName = '';
    masterTagName = '';
    productCategoryName = '';
    currencyName = '';
    businessName = '';
    countryName = '';
    stateName = '';
    cityName = '';
    jobStatusTypeName = '';
    storeName = '';

    constructor(
        injector: Injector,
        private _jobsServiceProxy: JobsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(jobId?: number): void {
        if (!jobId) {
            this.job = new CreateOrEditJobDto();
            this.job.id = jobId;
            this.job.startDate = this._dateTimeService.getStartOfDay();
            this.job.hireByDate = this._dateTimeService.getStartOfDay();
            this.job.publishDate = this._dateTimeService.getStartOfDay();
            this.job.expirationDate = this._dateTimeService.getStartOfDay();
            this.masterTagCategoryName = '';
            this.masterTagName = '';
            this.productCategoryName = '';
            this.currencyName = '';
            this.businessName = '';
            this.countryName = '';
            this.stateName = '';
            this.cityName = '';
            this.jobStatusTypeName = '';
            this.storeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._jobsServiceProxy.getJobForEdit(jobId).subscribe((result) => {
                this.job = result.job;

                this.masterTagCategoryName = result.masterTagCategoryName;
                this.masterTagName = result.masterTagName;
                this.productCategoryName = result.productCategoryName;
                this.currencyName = result.currencyName;
                this.businessName = result.businessName;
                this.countryName = result.countryName;
                this.stateName = result.stateName;
                this.cityName = result.cityName;
                this.jobStatusTypeName = result.jobStatusTypeName;
                this.storeName = result.storeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._jobsServiceProxy
            .createOrEdit(this.job)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    openSelectMasterTagCategoryModal() {
        this.jobMasterTagCategoryLookupTableModal.id = this.job.masterTagCategoryId;
        this.jobMasterTagCategoryLookupTableModal.displayName = this.masterTagCategoryName;
        this.jobMasterTagCategoryLookupTableModal.show();
    }
    openSelectMasterTagModal() {
        this.jobMasterTagLookupTableModal.id = this.job.masterTagId;
        this.jobMasterTagLookupTableModal.displayName = this.masterTagName;
        this.jobMasterTagLookupTableModal.show();
    }
    openSelectProductCategoryModal() {
        this.jobProductCategoryLookupTableModal.id = this.job.productCategoryId;
        this.jobProductCategoryLookupTableModal.displayName = this.productCategoryName;
        this.jobProductCategoryLookupTableModal.show();
    }
    openSelectCurrencyModal() {
        this.jobCurrencyLookupTableModal.id = this.job.currencyId;
        this.jobCurrencyLookupTableModal.displayName = this.currencyName;
        this.jobCurrencyLookupTableModal.show();
    }
    openSelectBusinessModal() {
        this.jobBusinessLookupTableModal.id = this.job.businessId;
        this.jobBusinessLookupTableModal.displayName = this.businessName;
        this.jobBusinessLookupTableModal.show();
    }
    openSelectCountryModal() {
        this.jobCountryLookupTableModal.id = this.job.countryId;
        this.jobCountryLookupTableModal.displayName = this.countryName;
        this.jobCountryLookupTableModal.show();
    }
    openSelectStateModal() {
        this.jobStateLookupTableModal.id = this.job.stateId;
        this.jobStateLookupTableModal.displayName = this.stateName;
        this.jobStateLookupTableModal.show();
    }
    openSelectCityModal() {
        this.jobCityLookupTableModal.id = this.job.cityId;
        this.jobCityLookupTableModal.displayName = this.cityName;
        this.jobCityLookupTableModal.show();
    }
    openSelectJobStatusTypeModal() {
        this.jobJobStatusTypeLookupTableModal.id = this.job.jobStatusTypeId;
        this.jobJobStatusTypeLookupTableModal.displayName = this.jobStatusTypeName;
        this.jobJobStatusTypeLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.jobStoreLookupTableModal.id = this.job.storeId;
        this.jobStoreLookupTableModal.displayName = this.storeName;
        this.jobStoreLookupTableModal.show();
    }

    setMasterTagCategoryIdNull() {
        this.job.masterTagCategoryId = null;
        this.masterTagCategoryName = '';
    }
    setMasterTagIdNull() {
        this.job.masterTagId = null;
        this.masterTagName = '';
    }
    setProductCategoryIdNull() {
        this.job.productCategoryId = null;
        this.productCategoryName = '';
    }
    setCurrencyIdNull() {
        this.job.currencyId = null;
        this.currencyName = '';
    }
    setBusinessIdNull() {
        this.job.businessId = null;
        this.businessName = '';
    }
    setCountryIdNull() {
        this.job.countryId = null;
        this.countryName = '';
    }
    setStateIdNull() {
        this.job.stateId = null;
        this.stateName = '';
    }
    setCityIdNull() {
        this.job.cityId = null;
        this.cityName = '';
    }
    setJobStatusTypeIdNull() {
        this.job.jobStatusTypeId = null;
        this.jobStatusTypeName = '';
    }
    setStoreIdNull() {
        this.job.storeId = null;
        this.storeName = '';
    }

    getNewMasterTagCategoryId() {
        this.job.masterTagCategoryId = this.jobMasterTagCategoryLookupTableModal.id;
        this.masterTagCategoryName = this.jobMasterTagCategoryLookupTableModal.displayName;
    }
    getNewMasterTagId() {
        this.job.masterTagId = this.jobMasterTagLookupTableModal.id;
        this.masterTagName = this.jobMasterTagLookupTableModal.displayName;
    }
    getNewProductCategoryId() {
        this.job.productCategoryId = this.jobProductCategoryLookupTableModal.id;
        this.productCategoryName = this.jobProductCategoryLookupTableModal.displayName;
    }
    getNewCurrencyId() {
        this.job.currencyId = this.jobCurrencyLookupTableModal.id;
        this.currencyName = this.jobCurrencyLookupTableModal.displayName;
    }
    getNewBusinessId() {
        this.job.businessId = this.jobBusinessLookupTableModal.id;
        this.businessName = this.jobBusinessLookupTableModal.displayName;
    }
    getNewCountryId() {
        this.job.countryId = this.jobCountryLookupTableModal.id;
        this.countryName = this.jobCountryLookupTableModal.displayName;
    }
    getNewStateId() {
        this.job.stateId = this.jobStateLookupTableModal.id;
        this.stateName = this.jobStateLookupTableModal.displayName;
    }
    getNewCityId() {
        this.job.cityId = this.jobCityLookupTableModal.id;
        this.cityName = this.jobCityLookupTableModal.displayName;
    }
    getNewJobStatusTypeId() {
        this.job.jobStatusTypeId = this.jobJobStatusTypeLookupTableModal.id;
        this.jobStatusTypeName = this.jobJobStatusTypeLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.job.storeId = this.jobStoreLookupTableModal.id;
        this.storeName = this.jobStoreLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
