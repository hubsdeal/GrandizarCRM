import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { JobHiringTeamsServiceProxy, JobHiringTeamDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditJobHiringTeamModalComponent } from './create-or-edit-jobHiringTeam-modal.component';

import { ViewJobHiringTeamModalComponent } from './view-jobHiringTeam-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './jobHiringTeams.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class JobHiringTeamsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditJobHiringTeamModal', { static: true }) createOrEditJobHiringTeamModal: CreateOrEditJobHiringTeamModalComponent;
    @ViewChild('viewJobHiringTeamModal', { static: true }) viewJobHiringTeamModal: ViewJobHiringTeamModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    firstNameFilter = '';
    lastNameFilter = '';
    jobTitleFilter = '';
    phoneFilter = '';
    emailFilter = '';
    hiringManagerFilter = -1;
        businessNameFilter = '';
        contactFullNameFilter = '';
        employeeNameFilter = '';






    constructor(
        injector: Injector,
        private _jobHiringTeamsServiceProxy: JobHiringTeamsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getJobHiringTeams(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._jobHiringTeamsServiceProxy.getAll(
            this.filterText,
            this.firstNameFilter,
            this.lastNameFilter,
            this.jobTitleFilter,
            this.phoneFilter,
            this.emailFilter,
            this.hiringManagerFilter,
            this.businessNameFilter,
            this.contactFullNameFilter,
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

    createJobHiringTeam(): void {
        this.createOrEditJobHiringTeamModal.show();        
    }


    deleteJobHiringTeam(jobHiringTeam: JobHiringTeamDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._jobHiringTeamsServiceProxy.delete(jobHiringTeam.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._jobHiringTeamsServiceProxy.getJobHiringTeamsToExcel(
        this.filterText,
            this.firstNameFilter,
            this.lastNameFilter,
            this.jobTitleFilter,
            this.phoneFilter,
            this.emailFilter,
            this.hiringManagerFilter,
            this.businessNameFilter,
            this.contactFullNameFilter,
            this.employeeNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.firstNameFilter = '';
    this.lastNameFilter = '';
    this.jobTitleFilter = '';
    this.phoneFilter = '';
    this.emailFilter = '';
    this.hiringManagerFilter = -1;
		this.businessNameFilter = '';
							this.contactFullNameFilter = '';
							this.employeeNameFilter = '';
							this.jobTitleFilter = '';
					
        this.getJobHiringTeams();
    }
}
