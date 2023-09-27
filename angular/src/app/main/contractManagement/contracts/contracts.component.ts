import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { ContractsServiceProxy, ContractDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditContractModalComponent } from './create-or-edit-contract-modal.component';

import { ViewContractModalComponent } from './view-contract-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './contracts.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ContractsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditContractModal', { static: true }) createOrEditContractModal: CreateOrEditContractModalComponent;
    @ViewChild('viewContractModal', { static: true }) viewContractModal: ViewContractModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    titleFilter = '';
    referenceNumberFilter = '';
    maxStartDateFilter : DateTime;
		minStartDateFilter : DateTime;
    maxEndDateFilter : DateTime;
		minEndDateFilter : DateTime;
    descriptionFilter = '';
    templateFilter = -1;
    remarksFilter = '';
    maxStoreMarketplaceCommissionSettingIdFilter : number;
		maxStoreMarketplaceCommissionSettingIdFilterEmpty : number;
		minStoreMarketplaceCommissionSettingIdFilter : number;
		minStoreMarketplaceCommissionSettingIdFilterEmpty : number;
    firstPartyFullNameFilter = '';
    firstPartyAddressFilter = '';
    firstPartyPhoneFilter = '';
    firstPartyEmailAddressFilter = '';
    firstPartyIdTypeAndNumberFilter = '';
    maxFirstPartySignDateFilter : DateTime;
		minFirstPartySignDateFilter : DateTime;
    firstPartySignBinaryIdFilter = '';
    firstPartyIpAddressFilter = '';
    secondPartyFullNameFilter = '';
    secondPartyAddressFilter = '';
    secondPartyPhoneFilter = '';
    secondPartyEmailAddressFilter = '';
    secondPartyIdTypeAndNumberFilter = '';
    maxSecondPartySignDateFilter : DateTime;
		minSecondPartySignDateFilter : DateTime;
    secondPartySignBinaryIdFilter = '';
    secondPartyIpAddressFilter = '';
        contractTypeNameFilter = '';
        storeNameFilter = '';
        businessNameFilter = '';
        employeeNameFilter = '';
        jobTitleFilter = '';
        productNameFilter = '';
        hubNameFilter = '';
        contactFullNameFilter = '';
        contactFullName2Filter = '';






    constructor(
        injector: Injector,
        private _contractsServiceProxy: ContractsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getContracts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._contractsServiceProxy.getAll(
            this.filterText,
            this.titleFilter,
            this.referenceNumberFilter,
            this.maxStartDateFilter === undefined ? this.maxStartDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
            this.minStartDateFilter === undefined ? this.minStartDateFilter : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
            this.maxEndDateFilter === undefined ? this.maxEndDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
            this.minEndDateFilter === undefined ? this.minEndDateFilter : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
            this.descriptionFilter,
            this.templateFilter,
            this.remarksFilter,
            this.maxStoreMarketplaceCommissionSettingIdFilter == null ? this.maxStoreMarketplaceCommissionSettingIdFilterEmpty: this.maxStoreMarketplaceCommissionSettingIdFilter,
            this.minStoreMarketplaceCommissionSettingIdFilter == null ? this.minStoreMarketplaceCommissionSettingIdFilterEmpty: this.minStoreMarketplaceCommissionSettingIdFilter,
            this.firstPartyFullNameFilter,
            this.firstPartyAddressFilter,
            this.firstPartyPhoneFilter,
            this.firstPartyEmailAddressFilter,
            this.firstPartyIdTypeAndNumberFilter,
            this.maxFirstPartySignDateFilter === undefined ? this.maxFirstPartySignDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxFirstPartySignDateFilter),
            this.minFirstPartySignDateFilter === undefined ? this.minFirstPartySignDateFilter : this._dateTimeService.getStartOfDayForDate(this.minFirstPartySignDateFilter),
            this.firstPartySignBinaryIdFilter,
            this.firstPartyIpAddressFilter,
            this.secondPartyFullNameFilter,
            this.secondPartyAddressFilter,
            this.secondPartyPhoneFilter,
            this.secondPartyEmailAddressFilter,
            this.secondPartyIdTypeAndNumberFilter,
            this.maxSecondPartySignDateFilter === undefined ? this.maxSecondPartySignDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxSecondPartySignDateFilter),
            this.minSecondPartySignDateFilter === undefined ? this.minSecondPartySignDateFilter : this._dateTimeService.getStartOfDayForDate(this.minSecondPartySignDateFilter),
            this.secondPartySignBinaryIdFilter,
            this.secondPartyIpAddressFilter,
            this.contractTypeNameFilter,
            this.storeNameFilter,
            this.businessNameFilter,
            this.employeeNameFilter,
            this.jobTitleFilter,
            this.productNameFilter,
            this.hubNameFilter,
            this.contactFullNameFilter,
            this.contactFullName2Filter,
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

    createContract(): void {
        this.createOrEditContractModal.show();        
    }


    deleteContract(contract: ContractDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._contractsServiceProxy.delete(contract.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._contractsServiceProxy.getContractsToExcel(
        this.filterText,
            this.titleFilter,
            this.referenceNumberFilter,
            this.maxStartDateFilter === undefined ? this.maxStartDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
            this.minStartDateFilter === undefined ? this.minStartDateFilter : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
            this.maxEndDateFilter === undefined ? this.maxEndDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
            this.minEndDateFilter === undefined ? this.minEndDateFilter : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
            this.descriptionFilter,
            this.templateFilter,
            this.remarksFilter,
            this.maxStoreMarketplaceCommissionSettingIdFilter == null ? this.maxStoreMarketplaceCommissionSettingIdFilterEmpty: this.maxStoreMarketplaceCommissionSettingIdFilter,
            this.minStoreMarketplaceCommissionSettingIdFilter == null ? this.minStoreMarketplaceCommissionSettingIdFilterEmpty: this.minStoreMarketplaceCommissionSettingIdFilter,
            this.firstPartyFullNameFilter,
            this.firstPartyAddressFilter,
            this.firstPartyPhoneFilter,
            this.firstPartyEmailAddressFilter,
            this.firstPartyIdTypeAndNumberFilter,
            this.maxFirstPartySignDateFilter === undefined ? this.maxFirstPartySignDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxFirstPartySignDateFilter),
            this.minFirstPartySignDateFilter === undefined ? this.minFirstPartySignDateFilter : this._dateTimeService.getStartOfDayForDate(this.minFirstPartySignDateFilter),
            this.firstPartySignBinaryIdFilter,
            this.firstPartyIpAddressFilter,
            this.secondPartyFullNameFilter,
            this.secondPartyAddressFilter,
            this.secondPartyPhoneFilter,
            this.secondPartyEmailAddressFilter,
            this.secondPartyIdTypeAndNumberFilter,
            this.maxSecondPartySignDateFilter === undefined ? this.maxSecondPartySignDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxSecondPartySignDateFilter),
            this.minSecondPartySignDateFilter === undefined ? this.minSecondPartySignDateFilter : this._dateTimeService.getStartOfDayForDate(this.minSecondPartySignDateFilter),
            this.secondPartySignBinaryIdFilter,
            this.secondPartyIpAddressFilter,
            this.contractTypeNameFilter,
            this.storeNameFilter,
            this.businessNameFilter,
            this.employeeNameFilter,
            this.jobTitleFilter,
            this.productNameFilter,
            this.hubNameFilter,
            this.contactFullNameFilter,
            this.contactFullName2Filter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.titleFilter = '';
    this.referenceNumberFilter = '';
    this.maxStartDateFilter = undefined;
		this.minStartDateFilter = undefined;
    this.maxEndDateFilter = undefined;
		this.minEndDateFilter = undefined;
    this.descriptionFilter = '';
    this.templateFilter = -1;
    this.remarksFilter = '';
    this.maxStoreMarketplaceCommissionSettingIdFilter = this.maxStoreMarketplaceCommissionSettingIdFilterEmpty;
		this.minStoreMarketplaceCommissionSettingIdFilter = this.maxStoreMarketplaceCommissionSettingIdFilterEmpty;
    this.firstPartyFullNameFilter = '';
    this.firstPartyAddressFilter = '';
    this.firstPartyPhoneFilter = '';
    this.firstPartyEmailAddressFilter = '';
    this.firstPartyIdTypeAndNumberFilter = '';
    this.maxFirstPartySignDateFilter = undefined;
		this.minFirstPartySignDateFilter = undefined;
    this.firstPartySignBinaryIdFilter = '';
    this.firstPartyIpAddressFilter = '';
    this.secondPartyFullNameFilter = '';
    this.secondPartyAddressFilter = '';
    this.secondPartyPhoneFilter = '';
    this.secondPartyEmailAddressFilter = '';
    this.secondPartyIdTypeAndNumberFilter = '';
    this.maxSecondPartySignDateFilter = undefined;
		this.minSecondPartySignDateFilter = undefined;
    this.secondPartySignBinaryIdFilter = '';
    this.secondPartyIpAddressFilter = '';
		this.contractTypeNameFilter = '';
							this.storeNameFilter = '';
							this.businessNameFilter = '';
							this.employeeNameFilter = '';
							this.jobTitleFilter = '';
							this.productNameFilter = '';
							this.hubNameFilter = '';
							this.contactFullNameFilter = '';
							this.contactFullName2Filter = '';
					
        this.getContracts();
    }
}
