import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { ContractFinancialTermsServiceProxy, ContractFinancialTermDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditContractFinancialTermModalComponent } from './create-or-edit-contractFinancialTerm-modal.component';

import { ViewContractFinancialTermModalComponent } from './view-contractFinancialTerm-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './contractFinancialTerms.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ContractFinancialTermsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditContractFinancialTermModal', { static: true }) createOrEditContractFinancialTermModal: CreateOrEditContractFinancialTermModalComponent;
    @ViewChild('viewContractFinancialTermModal', { static: true }) viewContractFinancialTermModal: ViewContractFinancialTermModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxPercentageFilter : number;
		maxPercentageFilterEmpty : number;
		minPercentageFilter : number;
		minPercentageFilterEmpty : number;
    maxFixedAmountFilter : number;
		maxFixedAmountFilterEmpty : number;
		minFixedAmountFilter : number;
		minFixedAmountFilterEmpty : number;
    maxStartDateFilter : DateTime;
		minStartDateFilter : DateTime;
    maxEndDateFilter : DateTime;
		minEndDateFilter : DateTime;
    isActiveFilter = -1;
        contractFirstPartyFullNameFilter = '';
        hubNameFilter = '';
        businessNameFilter = '';
        storeNameFilter = '';
        jobTitleFilter = '';
        productCategoryNameFilter = '';
        productNameFilter = '';






    constructor(
        injector: Injector,
        private _contractFinancialTermsServiceProxy: ContractFinancialTermsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getContractFinancialTerms(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._contractFinancialTermsServiceProxy.getAll(
            this.filterText,
            this.maxPercentageFilter == null ? this.maxPercentageFilterEmpty: this.maxPercentageFilter,
            this.minPercentageFilter == null ? this.minPercentageFilterEmpty: this.minPercentageFilter,
            this.maxFixedAmountFilter == null ? this.maxFixedAmountFilterEmpty: this.maxFixedAmountFilter,
            this.minFixedAmountFilter == null ? this.minFixedAmountFilterEmpty: this.minFixedAmountFilter,
            this.maxStartDateFilter === undefined ? this.maxStartDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
            this.minStartDateFilter === undefined ? this.minStartDateFilter : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
            this.maxEndDateFilter === undefined ? this.maxEndDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
            this.minEndDateFilter === undefined ? this.minEndDateFilter : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
            this.isActiveFilter,
            this.contractFirstPartyFullNameFilter,
            this.hubNameFilter,
            this.businessNameFilter,
            this.storeNameFilter,
            this.jobTitleFilter,
            this.productCategoryNameFilter,
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

    createContractFinancialTerm(): void {
        this.createOrEditContractFinancialTermModal.show();        
    }


    deleteContractFinancialTerm(contractFinancialTerm: ContractFinancialTermDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._contractFinancialTermsServiceProxy.delete(contractFinancialTerm.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._contractFinancialTermsServiceProxy.getContractFinancialTermsToExcel(
        this.filterText,
            this.maxPercentageFilter == null ? this.maxPercentageFilterEmpty: this.maxPercentageFilter,
            this.minPercentageFilter == null ? this.minPercentageFilterEmpty: this.minPercentageFilter,
            this.maxFixedAmountFilter == null ? this.maxFixedAmountFilterEmpty: this.maxFixedAmountFilter,
            this.minFixedAmountFilter == null ? this.minFixedAmountFilterEmpty: this.minFixedAmountFilter,
            this.maxStartDateFilter === undefined ? this.maxStartDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
            this.minStartDateFilter === undefined ? this.minStartDateFilter : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
            this.maxEndDateFilter === undefined ? this.maxEndDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
            this.minEndDateFilter === undefined ? this.minEndDateFilter : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
            this.isActiveFilter,
            this.contractFirstPartyFullNameFilter,
            this.hubNameFilter,
            this.businessNameFilter,
            this.storeNameFilter,
            this.jobTitleFilter,
            this.productCategoryNameFilter,
            this.productNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.maxPercentageFilter = this.maxPercentageFilterEmpty;
		this.minPercentageFilter = this.maxPercentageFilterEmpty;
    this.maxFixedAmountFilter = this.maxFixedAmountFilterEmpty;
		this.minFixedAmountFilter = this.maxFixedAmountFilterEmpty;
    this.maxStartDateFilter = undefined;
		this.minStartDateFilter = undefined;
    this.maxEndDateFilter = undefined;
		this.minEndDateFilter = undefined;
    this.isActiveFilter = -1;
		this.contractFirstPartyFullNameFilter = '';
							this.hubNameFilter = '';
							this.businessNameFilter = '';
							this.storeNameFilter = '';
							this.jobTitleFilter = '';
							this.productCategoryNameFilter = '';
							this.productNameFilter = '';
					
        this.getContractFinancialTerms();
    }
}
