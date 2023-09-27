import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { ContactOrApplicantTeamsServiceProxy, ContactOrApplicantTeamDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditContactOrApplicantTeamModalComponent } from './create-or-edit-contactOrApplicantTeam-modal.component';

import { ViewContactOrApplicantTeamModalComponent } from './view-contactOrApplicantTeam-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './contactOrApplicantTeams.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ContactOrApplicantTeamsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditContactOrApplicantTeamModal', { static: true }) createOrEditContactOrApplicantTeamModal: CreateOrEditContactOrApplicantTeamModalComponent;
    @ViewChild('viewContactOrApplicantTeamModal', { static: true }) viewContactOrApplicantTeamModal: ViewContactOrApplicantTeamModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    primaryFilter = -1;
        contactFullNameFilter = '';
        employeeNameFilter = '';






    constructor(
        injector: Injector,
        private _contactOrApplicantTeamsServiceProxy: ContactOrApplicantTeamsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getContactOrApplicantTeams(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._contactOrApplicantTeamsServiceProxy.getAll(
            this.filterText,
            this.primaryFilter,
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

    createContactOrApplicantTeam(): void {
        this.createOrEditContactOrApplicantTeamModal.show();        
    }


    deleteContactOrApplicantTeam(contactOrApplicantTeam: ContactOrApplicantTeamDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._contactOrApplicantTeamsServiceProxy.delete(contactOrApplicantTeam.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._contactOrApplicantTeamsServiceProxy.getContactOrApplicantTeamsToExcel(
        this.filterText,
            this.primaryFilter,
            this.contactFullNameFilter,
            this.employeeNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.primaryFilter = -1;
		this.contactFullNameFilter = '';
							this.employeeNameFilter = '';
					
        this.getContactOrApplicantTeams();
    }
}
