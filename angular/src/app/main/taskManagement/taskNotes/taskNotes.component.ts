import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input, OnInit } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
// import { TaskNotesServiceProxy, TaskNoteDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TaskNoteDto, TaskNotesServiceProxy, TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTaskNoteModalComponent } from './create-or-edit-taskNote-modal.component';

import { ViewTaskNoteModalComponent } from './view-taskNote-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'app-taskNotes',
    templateUrl: './taskNotes.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TaskNotesComponent extends AppComponentBase implements OnInit  {
    
    
    @ViewChild('createOrEditTaskNoteModal', { static: true }) createOrEditTaskNoteModal: CreateOrEditTaskNoteModalComponent;
    @ViewChild('viewTaskNoteModal', { static: true }) viewTaskNoteModal: ViewTaskNoteModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    noteFilter = '';
        taskEventNameFilter = '';


        @Input() taskEventId: number;

        allTaskNotes: any[] = [];



    constructor(
        injector: Injector,
        private _taskNotesServiceProxy: TaskNotesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this.getAllTaskNotes(this.taskEventId);
    }

    getAllTaskNotes(id: number) {
        this._taskNotesServiceProxy.getAllByTaskEventId(
            this.taskEventId,
            this.filterText,
            this.noteFilter,
            this.taskEventNameFilter,
            '',
            0,
            10000
        ).subscribe(result => {
            this.allTaskNotes = result.items;
           
        });
    }
  
    getTaskNotes(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        // this._taskNotesServiceProxy.getAll(
        //     this.filterText,
        //     this.noteFilter,
        //     this.taskEventNameFilter,
        //     this.primengTableHelper.getSorting(this.dataTable),
        //     this.primengTableHelper.getSkipCount(this.paginator, event),
        //     this.primengTableHelper.getMaxResultCount(this.paginator, event)
        // ).subscribe(result => {
        //     this.primengTableHelper.totalRecordsCount = result.totalCount;
        //     this.primengTableHelper.records = result.items;
        //     this.primengTableHelper.hideLoadingIndicator();
        // });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createTaskNote(): void {
        this.createOrEditTaskNoteModal.show();  
        this.createOrEditTaskNoteModal.taskId = this.taskEventId;      
    }


    deleteTaskNote(taskNote: TaskNoteDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._taskNotesServiceProxy.delete(taskNote.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._taskNotesServiceProxy.getTaskNotesToExcel(
        this.filterText,
            this.noteFilter,
            this.taskEventNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.noteFilter = '';
		this.taskEventNameFilter = '';
					
        this.getAllTaskNotes(this.taskEventId);
    }
}
