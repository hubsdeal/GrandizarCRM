import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ZipCodesServiceProxy, ZipCodeDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditZipCodeModalComponent } from './create-or-edit-zipCode-modal.component';

import { ViewZipCodeModalComponent } from './view-zipCode-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './zipCodes.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ZipCodesComponent extends AppComponentBase {
    @ViewChild('createOrEditZipCodeModal', { static: true })
    createOrEditZipCodeModal: CreateOrEditZipCodeModalComponent;
    @ViewChild('viewZipCodeModal', { static: true }) viewZipCodeModal: ViewZipCodeModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    areaCodeFilter = '';
    asianPopulationFilter = '';
    averageHouseValueFilter = '';
    blackPopulationFilter = '';
    cbsaFilter = '';
    cbsA_DivFilter = '';
    cbsA_Div_NameFilter = '';
    cbsA_NameFilter = '';
    cbsA_TypeFilter = '';
    csaFilter = '';
    csaNameFilter = '';
    carrierRouteRateSortationFilter = '';
    cityFilter = '';
    cityAliasCodeFilter = '';
    cityAliasMixedCaseFilter = '';
    cityAliasNameFilter = '';
    cityDeliveryIndicatorFilter = '';
    cityMixedCaseFilter = '';
    cityStateKeyFilter = '';
    cityTypeFilter = '';
    classificationCodeFilter = '';
    countyFilter = '';
    countyANSIFilter = '';
    countyFIPSFilter = '';
    countyMixedCaseFilter = '';
    dayLightSavingFilter = '';
    divisionFilter = '';
    elevationFilter = '';
    facilityCodeFilter = '';
    femalePopulationFilter = '';
    financeNumberFilter = '';
    hawaiianPopulationFilter = '';
    hispanicPopulationFilter = '';
    householdsPerZipCodeFilter = '';
    incomePerHouseholdFilter = '';
    indianPopulationFilter = '';
    latitudeFilter = '';
    longitudeFilter = '';
    msaFilter = '';
    msA_NameFilter = '';
    mailingNameFilter = '';
    malePopulationFilter = '';
    multiCountyFilter = '';
    otherPopulationFilter = '';
    pmsaFilter = '';
    pmsA_NameFilter = '';
    personsPerHouseholdFilter = '';
    populationFilter = '';
    preferredLastLineKeyFilter = '';
    primaryRecordFilter = '';
    regionFilter = '';
    stateFilter = '';
    stateANSIFilter = '';
    stateFIPSFilter = '';
    stateFullNameFilter = '';
    timeZoneFilter = '';
    uniqueZIPNameFilter = '';
    whitePopulationFilter = '';
    countryNameFilter = '';
    stateNameFilter = '';
    cityNameFilter = '';
    countyNameFilter = '';

    constructor(
        injector: Injector,
        private _zipCodesServiceProxy: ZipCodesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getZipCodes(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._zipCodesServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.areaCodeFilter,
                this.asianPopulationFilter,
                this.averageHouseValueFilter,
                this.blackPopulationFilter,
                this.cbsaFilter,
                this.cbsA_DivFilter,
                this.cbsA_Div_NameFilter,
                this.cbsA_NameFilter,
                this.cbsA_TypeFilter,
                this.csaFilter,
                this.csaNameFilter,
                this.carrierRouteRateSortationFilter,
                this.cityFilter,
                this.cityAliasCodeFilter,
                this.cityAliasMixedCaseFilter,
                this.cityAliasNameFilter,
                this.cityDeliveryIndicatorFilter,
                this.cityMixedCaseFilter,
                this.cityStateKeyFilter,
                this.cityTypeFilter,
                this.classificationCodeFilter,
                this.countyFilter,
                this.countyANSIFilter,
                this.countyFIPSFilter,
                this.countyMixedCaseFilter,
                this.dayLightSavingFilter,
                this.divisionFilter,
                this.elevationFilter,
                this.facilityCodeFilter,
                this.femalePopulationFilter,
                this.financeNumberFilter,
                this.hawaiianPopulationFilter,
                this.hispanicPopulationFilter,
                this.householdsPerZipCodeFilter,
                this.incomePerHouseholdFilter,
                this.indianPopulationFilter,
                this.latitudeFilter,
                this.longitudeFilter,
                this.msaFilter,
                this.msA_NameFilter,
                this.mailingNameFilter,
                this.malePopulationFilter,
                this.multiCountyFilter,
                this.otherPopulationFilter,
                this.pmsaFilter,
                this.pmsA_NameFilter,
                this.personsPerHouseholdFilter,
                this.populationFilter,
                this.preferredLastLineKeyFilter,
                this.primaryRecordFilter,
                this.regionFilter,
                this.stateFilter,
                this.stateANSIFilter,
                this.stateFIPSFilter,
                this.stateFullNameFilter,
                this.timeZoneFilter,
                this.uniqueZIPNameFilter,
                this.whitePopulationFilter,
                this.countryNameFilter,
                this.stateNameFilter,
                this.cityNameFilter,
                this.countyNameFilter,
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

    createZipCode(): void {
        this.createOrEditZipCodeModal.show();
    }

    deleteZipCode(zipCode: ZipCodeDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._zipCodesServiceProxy.delete(zipCode.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._zipCodesServiceProxy
            .getZipCodesToExcel(
                this.filterText,
                this.nameFilter,
                this.areaCodeFilter,
                this.asianPopulationFilter,
                this.averageHouseValueFilter,
                this.blackPopulationFilter,
                this.cbsaFilter,
                this.cbsA_DivFilter,
                this.cbsA_Div_NameFilter,
                this.cbsA_NameFilter,
                this.cbsA_TypeFilter,
                this.csaFilter,
                this.csaNameFilter,
                this.carrierRouteRateSortationFilter,
                this.cityFilter,
                this.cityAliasCodeFilter,
                this.cityAliasMixedCaseFilter,
                this.cityAliasNameFilter,
                this.cityDeliveryIndicatorFilter,
                this.cityMixedCaseFilter,
                this.cityStateKeyFilter,
                this.cityTypeFilter,
                this.classificationCodeFilter,
                this.countyFilter,
                this.countyANSIFilter,
                this.countyFIPSFilter,
                this.countyMixedCaseFilter,
                this.dayLightSavingFilter,
                this.divisionFilter,
                this.elevationFilter,
                this.facilityCodeFilter,
                this.femalePopulationFilter,
                this.financeNumberFilter,
                this.hawaiianPopulationFilter,
                this.hispanicPopulationFilter,
                this.householdsPerZipCodeFilter,
                this.incomePerHouseholdFilter,
                this.indianPopulationFilter,
                this.latitudeFilter,
                this.longitudeFilter,
                this.msaFilter,
                this.msA_NameFilter,
                this.mailingNameFilter,
                this.malePopulationFilter,
                this.multiCountyFilter,
                this.otherPopulationFilter,
                this.pmsaFilter,
                this.pmsA_NameFilter,
                this.personsPerHouseholdFilter,
                this.populationFilter,
                this.preferredLastLineKeyFilter,
                this.primaryRecordFilter,
                this.regionFilter,
                this.stateFilter,
                this.stateANSIFilter,
                this.stateFIPSFilter,
                this.stateFullNameFilter,
                this.timeZoneFilter,
                this.uniqueZIPNameFilter,
                this.whitePopulationFilter,
                this.countryNameFilter,
                this.stateNameFilter,
                this.cityNameFilter,
                this.countyNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.nameFilter = '';
        this.areaCodeFilter = '';
        this.asianPopulationFilter = '';
        this.averageHouseValueFilter = '';
        this.blackPopulationFilter = '';
        this.cbsaFilter = '';
        this.cbsA_DivFilter = '';
        this.cbsA_Div_NameFilter = '';
        this.cbsA_NameFilter = '';
        this.cbsA_TypeFilter = '';
        this.csaFilter = '';
        this.csaNameFilter = '';
        this.carrierRouteRateSortationFilter = '';
        this.cityFilter = '';
        this.cityAliasCodeFilter = '';
        this.cityAliasMixedCaseFilter = '';
        this.cityAliasNameFilter = '';
        this.cityDeliveryIndicatorFilter = '';
        this.cityMixedCaseFilter = '';
        this.cityStateKeyFilter = '';
        this.cityTypeFilter = '';
        this.classificationCodeFilter = '';
        this.countyFilter = '';
        this.countyANSIFilter = '';
        this.countyFIPSFilter = '';
        this.countyMixedCaseFilter = '';
        this.dayLightSavingFilter = '';
        this.divisionFilter = '';
        this.elevationFilter = '';
        this.facilityCodeFilter = '';
        this.femalePopulationFilter = '';
        this.financeNumberFilter = '';
        this.hawaiianPopulationFilter = '';
        this.hispanicPopulationFilter = '';
        this.householdsPerZipCodeFilter = '';
        this.incomePerHouseholdFilter = '';
        this.indianPopulationFilter = '';
        this.latitudeFilter = '';
        this.longitudeFilter = '';
        this.msaFilter = '';
        this.msA_NameFilter = '';
        this.mailingNameFilter = '';
        this.malePopulationFilter = '';
        this.multiCountyFilter = '';
        this.otherPopulationFilter = '';
        this.pmsaFilter = '';
        this.pmsA_NameFilter = '';
        this.personsPerHouseholdFilter = '';
        this.populationFilter = '';
        this.preferredLastLineKeyFilter = '';
        this.primaryRecordFilter = '';
        this.regionFilter = '';
        this.stateFilter = '';
        this.stateANSIFilter = '';
        this.stateFIPSFilter = '';
        this.stateFullNameFilter = '';
        this.timeZoneFilter = '';
        this.uniqueZIPNameFilter = '';
        this.whitePopulationFilter = '';
        this.countryNameFilter = '';
        this.stateNameFilter = '';
        this.cityNameFilter = '';
        this.countyNameFilter = '';

        this.getZipCodes();
    }
}
