import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StoreNotesServiceProxy, StoreNoteDto, PublicPagesCommonServiceProxy } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreNoteModalComponent } from './create-or-edit-storeNote-modal.component';

import { ViewStoreNoteModalComponent } from './view-storeNote-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { moment } from 'ngx-bootstrap/chronos/testing/chain';

@Component({
    selector: 'app-storeNotes',
    templateUrl: './storeNotes.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoreNotesComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditStoreNoteModal', { static: true })
    createOrEditStoreNoteModal: CreateOrEditStoreNoteModalComponent;
    @ViewChild('viewStoreNoteModal', { static: true }) viewStoreNoteModal: ViewStoreNoteModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    notesFilter = '';
    storeNameFilter = '';

    @Input() storeId: number;

    allStoreNotes: any[] = [];

    constructor(
        injector: Injector,
        private _storeNotesServiceProxy: StoreNotesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _publicPagesCommonServicePrxoy:PublicPagesCommonServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
       
    }

    ngOnInit(): void {
        this.getAllStoreNotes(this.storeId);
    }

    getAllStoreNotes(id: number) {
        this._storeNotesServiceProxy.getAll('', '', '', id, '', 0, 1000).subscribe((result) => {
            this.allStoreNotes = result.items;
            console.log(this.allStoreNotes);
        });
    }
    getStoreNotes(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        // this._storeNotesServiceProxy
        //     .getAll(
        //         this.filterText,
        //         this.notesFilter,
        //         this.storeNameFilter,
        //         undefined,
        //         this.primengTableHelper.getSorting(this.dataTable),
        //         this.primengTableHelper.getSkipCount(this.paginator, event),
        //         this.primengTableHelper.getMaxResultCount(this.paginator, event)
        //     )
        //     .subscribe((result) => {
        //         this.primengTableHelper.totalRecordsCount = result.totalCount;
        //         this.primengTableHelper.records = result.items;
        //         this.primengTableHelper.hideLoadingIndicator();
        //     });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createStoreNote(): void {
        this.createOrEditStoreNoteModal.storeId = this.storeId;
        this.createOrEditStoreNoteModal.show();
    }

    deleteStoreNote(storeNote: StoreNoteDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._storeNotesServiceProxy.delete(storeNote.id).subscribe(() => {
                    this.getAllStoreNotes(this.storeId);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._storeNotesServiceProxy
            .getStoreNotesToExcel(this.filterText, this.notesFilter, this.storeNameFilter)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.notesFilter = '';
        this.storeNameFilter = '';

        this.getStoreNotes();
    }

    // creationDayCalcuation(date?: any): string {
    //     var taskAssignTimeLeft = moment().diff(date, 'days');
    //     if (taskAssignTimeLeft == 0) {
    //         taskAssignTimeLeft = moment().diff(date, 'minutes');
    //         if (taskAssignTimeLeft < 1) {
    //             var fewMinutesAgo = 'just now';
    //             return fewMinutesAgo;
    //         }
    //         else if (taskAssignTimeLeft < 5) {
    //             var fewMinutesAgo = 'few minutes ago';
    //             return fewMinutesAgo;
    //         }
    //         else if (taskAssignTimeLeft >= 59) {
    //             taskAssignTimeLeft = moment().diff(date, 'hours');
    //             var hourWord = taskAssignTimeLeft == 1 ? ' hour' : ' hours';
    //             return taskAssignTimeLeft + hourWord + " ago"
    //         }
    //         return taskAssignTimeLeft + " minutes ago"
    //     }

    //     var dayWord = taskAssignTimeLeft == 1 ? ' day' : ' days';
    //     return taskAssignTimeLeft.toString() + dayWord + " ago";
    // }
}
