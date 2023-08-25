import { Component, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ContactsServiceProxy } from '@shared/service-proxies/service-proxies';
import { DateTime } from 'luxon';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import {trim} from "lodash-es";


@Component({
    templateUrl: './applicantList.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./applicantList.component.scss'],
    animations: [appModuleAnimation()],
})
export class ApplicantListComponent extends AppComponentBase {

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

    checkIfAllSelected() {
     /*   this.selectedAll = this.primengTableHelper.records.every(function (item: any) {
            return item.selected == true;
        })*/
    }

    createApplicant() {

    }
    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }



    resetFilters() {

    }

    contactApplicantNameSplit(name: string): string {
        name = trim(name);
        var splitNames = name.split(" ");
        let characters = "";
        for (let i = 0; i < splitNames.length; i++) {
            splitNames[i] = trim(splitNames[i]);
            if (splitNames[i] != "") {
                characters += splitNames[i][0];
                if (characters.length > 1) {
                    break;
                }
            }

        }
        return characters;
    }

/*    calculateYearAndMonthDiff(isCurrent: boolean, startDate: moment.Moment, endDate: moment.Moment): string {
        startDate = startDate ?? moment(new Date());
        endDate = isCurrent ? moment(new Date()) : (endDate ?? moment(new Date()));
        var duration = moment.duration(endDate.diff(startDate));
        var years = duration.years();
        var month = duration.months();
        return years + ' Years ' + month + ' Months';

    }*/

    onConnectClick(id) {

    }
}
