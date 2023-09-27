import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { ConnectMessagesServiceProxy, ConnectMessageDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditConnectMessageModalComponent } from './create-or-edit-connectMessage-modal.component';

import { ViewConnectMessageModalComponent } from './view-connectMessage-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './connectMessages.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ConnectMessagesComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditConnectMessageModal', { static: true }) createOrEditConnectMessageModal: CreateOrEditConnectMessageModalComponent;
    @ViewChild('viewConnectMessageModal', { static: true }) viewConnectMessageModal: ViewConnectMessageModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    fromFilter = '';
    toFilter = '';
    subjectFilter = '';
    bodyFilter = '';
    sendOrReceiveFilter = -1;
    maxSendDateFilter : DateTime;
		minSendDateFilter : DateTime;
    sendTimeFilter = '';
    maxViewDateFilter : DateTime;
		minViewDateFilter : DateTime;
    viewTimeFilter = '';
    viewedFilter = -1;
    clickedFilter = -1;
    notesFilter = '';
    sendNowOrScheduledFilter = -1;
    maxScheduleSendDateTimeFilter : DateTime;
		minScheduleSendDateTimeFilter : DateTime;
        connectChannelNameFilter = '';
        userNameFilter = '';
        contactFullNameFilter = '';






    constructor(
        injector: Injector,
        private _connectMessagesServiceProxy: ConnectMessagesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getConnectMessages(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._connectMessagesServiceProxy.getAll(
            this.filterText,
            this.fromFilter,
            this.toFilter,
            this.subjectFilter,
            this.bodyFilter,
            this.sendOrReceiveFilter,
            this.maxSendDateFilter === undefined ? this.maxSendDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxSendDateFilter),
            this.minSendDateFilter === undefined ? this.minSendDateFilter : this._dateTimeService.getStartOfDayForDate(this.minSendDateFilter),
            this.sendTimeFilter,
            this.maxViewDateFilter === undefined ? this.maxViewDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxViewDateFilter),
            this.minViewDateFilter === undefined ? this.minViewDateFilter : this._dateTimeService.getStartOfDayForDate(this.minViewDateFilter),
            this.viewTimeFilter,
            this.viewedFilter,
            this.clickedFilter,
            this.notesFilter,
            this.sendNowOrScheduledFilter,
            this.maxScheduleSendDateTimeFilter === undefined ? this.maxScheduleSendDateTimeFilter : this._dateTimeService.getEndOfDayForDate(this.maxScheduleSendDateTimeFilter),
            this.minScheduleSendDateTimeFilter === undefined ? this.minScheduleSendDateTimeFilter : this._dateTimeService.getStartOfDayForDate(this.minScheduleSendDateTimeFilter),
            this.connectChannelNameFilter,
            this.userNameFilter,
            this.contactFullNameFilter,
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

    createConnectMessage(): void {
        this.createOrEditConnectMessageModal.show();        
    }


    deleteConnectMessage(connectMessage: ConnectMessageDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._connectMessagesServiceProxy.delete(connectMessage.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._connectMessagesServiceProxy.getConnectMessagesToExcel(
        this.filterText,
            this.fromFilter,
            this.toFilter,
            this.subjectFilter,
            this.bodyFilter,
            this.sendOrReceiveFilter,
            this.maxSendDateFilter === undefined ? this.maxSendDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxSendDateFilter),
            this.minSendDateFilter === undefined ? this.minSendDateFilter : this._dateTimeService.getStartOfDayForDate(this.minSendDateFilter),
            this.sendTimeFilter,
            this.maxViewDateFilter === undefined ? this.maxViewDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxViewDateFilter),
            this.minViewDateFilter === undefined ? this.minViewDateFilter : this._dateTimeService.getStartOfDayForDate(this.minViewDateFilter),
            this.viewTimeFilter,
            this.viewedFilter,
            this.clickedFilter,
            this.notesFilter,
            this.sendNowOrScheduledFilter,
            this.maxScheduleSendDateTimeFilter === undefined ? this.maxScheduleSendDateTimeFilter : this._dateTimeService.getEndOfDayForDate(this.maxScheduleSendDateTimeFilter),
            this.minScheduleSendDateTimeFilter === undefined ? this.minScheduleSendDateTimeFilter : this._dateTimeService.getStartOfDayForDate(this.minScheduleSendDateTimeFilter),
            this.connectChannelNameFilter,
            this.userNameFilter,
            this.contactFullNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.fromFilter = '';
    this.toFilter = '';
    this.subjectFilter = '';
    this.bodyFilter = '';
    this.sendOrReceiveFilter = -1;
    this.maxSendDateFilter = undefined;
		this.minSendDateFilter = undefined;
    this.sendTimeFilter = '';
    this.maxViewDateFilter = undefined;
		this.minViewDateFilter = undefined;
    this.viewTimeFilter = '';
    this.viewedFilter = -1;
    this.clickedFilter = -1;
    this.notesFilter = '';
    this.sendNowOrScheduledFilter = -1;
    this.maxScheduleSendDateTimeFilter = undefined;
		this.minScheduleSendDateTimeFilter = undefined;
		this.connectChannelNameFilter = '';
							this.userNameFilter = '';
							this.contactFullNameFilter = '';
					
        this.getConnectMessages();
    }
}
