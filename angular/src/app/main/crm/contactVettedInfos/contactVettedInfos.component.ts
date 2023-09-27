import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { ContactVettedInfosServiceProxy, ContactVettedInfoDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditContactVettedInfoModalComponent } from './create-or-edit-contactVettedInfo-modal.component';

import { ViewContactVettedInfoModalComponent } from './view-contactVettedInfo-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './contactVettedInfos.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ContactVettedInfosComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditContactVettedInfoModal', { static: true }) createOrEditContactVettedInfoModal: CreateOrEditContactVettedInfoModalComponent;
    @ViewChild('viewContactVettedInfoModal', { static: true }) viewContactVettedInfoModal: ViewContactVettedInfoModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    verificationStatusFilter = -1;
    managerRemarksFilter = '';
    maxDateVerificationFilter : DateTime;
		minDateVerificationFilter : DateTime;
        contactFullNameFilter = '';
        contactEducationNameFilter = '';
        contactExperienceJobTitleFilter = '';
        contactCertificationLicenseNameFilter = '';
        employeeNameFilter = '';






    constructor(
        injector: Injector,
        private _contactVettedInfosServiceProxy: ContactVettedInfosServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getContactVettedInfos(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._contactVettedInfosServiceProxy.getAll(
            this.filterText,
            this.verificationStatusFilter,
            this.managerRemarksFilter,
            this.maxDateVerificationFilter === undefined ? this.maxDateVerificationFilter : this._dateTimeService.getEndOfDayForDate(this.maxDateVerificationFilter),
            this.minDateVerificationFilter === undefined ? this.minDateVerificationFilter : this._dateTimeService.getStartOfDayForDate(this.minDateVerificationFilter),
            this.contactFullNameFilter,
            this.contactEducationNameFilter,
            this.contactExperienceJobTitleFilter,
            this.contactCertificationLicenseNameFilter,
            this.employeeNameFilter,
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

    createContactVettedInfo(): void {
        this.createOrEditContactVettedInfoModal.show();        
    }


    deleteContactVettedInfo(contactVettedInfo: ContactVettedInfoDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._contactVettedInfosServiceProxy.delete(contactVettedInfo.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._contactVettedInfosServiceProxy.getContactVettedInfosToExcel(
        this.filterText,
            this.verificationStatusFilter,
            this.managerRemarksFilter,
            this.maxDateVerificationFilter === undefined ? this.maxDateVerificationFilter : this._dateTimeService.getEndOfDayForDate(this.maxDateVerificationFilter),
            this.minDateVerificationFilter === undefined ? this.minDateVerificationFilter : this._dateTimeService.getStartOfDayForDate(this.minDateVerificationFilter),
            this.contactFullNameFilter,
            this.contactEducationNameFilter,
            this.contactExperienceJobTitleFilter,
            this.contactCertificationLicenseNameFilter,
            this.employeeNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.verificationStatusFilter = -1;
    this.managerRemarksFilter = '';
    this.maxDateVerificationFilter = undefined;
		this.minDateVerificationFilter = undefined;
		this.contactFullNameFilter = '';
							this.contactEducationNameFilter = '';
							this.contactExperienceJobTitleFilter = '';
							this.contactCertificationLicenseNameFilter = '';
							this.employeeNameFilter = '';
					
        this.getContactVettedInfos();
    }
}
