import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { JobApplicantHireMatrixesServiceProxy, JobApplicantHireMatrixDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditJobApplicantHireMatrixModalComponent } from './create-or-edit-jobApplicantHireMatrix-modal.component';

import { ViewJobApplicantHireMatrixModalComponent } from './view-jobApplicantHireMatrix-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './jobApplicantHireMatrixes.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class JobApplicantHireMatrixesComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditJobApplicantHireMatrixModal', { static: true }) createOrEditJobApplicantHireMatrixModal: CreateOrEditJobApplicantHireMatrixModalComponent;
    @ViewChild('viewJobApplicantHireMatrixModal', { static: true }) viewJobApplicantHireMatrixModal: ViewJobApplicantHireMatrixModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    matchedFilter = -1;
    thumbsUpOrDownFilter = -1;
    managerRemarksFilter = '';
    contactCoverLetterIdFilter = '';
    contactResumeIdFilter = '';
    contactCoverLetterNameFilter = '';
        jobTitleFilter = '';
        contactFullNameFilter = '';
        jobHiringTeamFirstNameFilter = '';
        jobApplicantHireStatusTypeNameFilter = '';






    constructor(
        injector: Injector,
        private _jobApplicantHireMatrixesServiceProxy: JobApplicantHireMatrixesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getJobApplicantHireMatrixes(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._jobApplicantHireMatrixesServiceProxy.getAll(
            this.filterText,
            this.matchedFilter,
            this.thumbsUpOrDownFilter,
            this.managerRemarksFilter,
            this.contactCoverLetterIdFilter,
            this.contactResumeIdFilter,
            this.contactCoverLetterNameFilter,
            this.jobTitleFilter,
            this.contactFullNameFilter,
            this.jobHiringTeamFirstNameFilter,
            this.jobApplicantHireStatusTypeNameFilter,
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

    createJobApplicantHireMatrix(): void {
        this.createOrEditJobApplicantHireMatrixModal.show();        
    }


    deleteJobApplicantHireMatrix(jobApplicantHireMatrix: JobApplicantHireMatrixDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._jobApplicantHireMatrixesServiceProxy.delete(jobApplicantHireMatrix.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._jobApplicantHireMatrixesServiceProxy.getJobApplicantHireMatrixesToExcel(
        this.filterText,
            this.matchedFilter,
            this.thumbsUpOrDownFilter,
            this.managerRemarksFilter,
            this.contactCoverLetterIdFilter,
            this.contactResumeIdFilter,
            this.contactCoverLetterNameFilter,
            this.jobTitleFilter,
            this.contactFullNameFilter,
            this.jobHiringTeamFirstNameFilter,
            this.jobApplicantHireStatusTypeNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.matchedFilter = -1;
    this.thumbsUpOrDownFilter = -1;
    this.managerRemarksFilter = '';
    this.contactCoverLetterIdFilter = '';
    this.contactResumeIdFilter = '';
    this.contactCoverLetterNameFilter = '';
		this.jobTitleFilter = '';
							this.contactFullNameFilter = '';
							this.jobHiringTeamFirstNameFilter = '';
							this.jobApplicantHireStatusTypeNameFilter = '';
					
        this.getJobApplicantHireMatrixes();
    }
}
