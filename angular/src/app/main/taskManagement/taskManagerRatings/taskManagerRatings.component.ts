import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { TaskManagerRatingsServiceProxy, TaskManagerRatingDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTaskManagerRatingModalComponent } from './create-or-edit-taskManagerRating-modal.component';

import { ViewTaskManagerRatingModalComponent } from './view-taskManagerRating-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './taskManagerRatings.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TaskManagerRatingsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditTaskManagerRatingModal', { static: true }) createOrEditTaskManagerRatingModal: CreateOrEditTaskManagerRatingModalComponent;
    @ViewChild('viewTaskManagerRatingModal', { static: true }) viewTaskManagerRatingModal: ViewTaskManagerRatingModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    managerFeedbackFilter = '';
        taskEventNameFilter = '';
        employeeNameFilter = '';
        taskTeamStartTimeFilter = '';
        ratingLikeNameFilter = '';






    constructor(
        injector: Injector,
        private _taskManagerRatingsServiceProxy: TaskManagerRatingsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getTaskManagerRatings(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._taskManagerRatingsServiceProxy.getAll(
            this.filterText,
            this.managerFeedbackFilter,
            this.taskEventNameFilter,
            this.employeeNameFilter,
            this.taskTeamStartTimeFilter,
            this.ratingLikeNameFilter,
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

    createTaskManagerRating(): void {
        this.createOrEditTaskManagerRatingModal.show();        
    }


    deleteTaskManagerRating(taskManagerRating: TaskManagerRatingDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._taskManagerRatingsServiceProxy.delete(taskManagerRating.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._taskManagerRatingsServiceProxy.getTaskManagerRatingsToExcel(
        this.filterText,
            this.managerFeedbackFilter,
            this.taskEventNameFilter,
            this.employeeNameFilter,
            this.taskTeamStartTimeFilter,
            this.ratingLikeNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.managerFeedbackFilter = '';
		this.taskEventNameFilter = '';
							this.employeeNameFilter = '';
							this.taskTeamStartTimeFilter = '';
							this.ratingLikeNameFilter = '';
					
        this.getTaskManagerRatings();
    }
}
