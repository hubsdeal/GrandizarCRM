import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ContactTaskMapsServiceProxy, ContactTaskMapDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditContactTaskMapModalComponent } from './create-or-edit-contactTaskMap-modal.component';

import { ViewContactTaskMapModalComponent } from './view-contactTaskMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './contactTaskMaps.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ContactTaskMapsComponent extends AppComponentBase {
    @ViewChild('createOrEditContactTaskMapModal', { static: true })
    createOrEditContactTaskMapModal: CreateOrEditContactTaskMapModalComponent;
    @ViewChild('viewContactTaskMapModal', { static: true }) viewContactTaskMapModal: ViewContactTaskMapModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    contactFullNameFilter = '';
    taskEventNameFilter = '';

    constructor(
        injector: Injector,
        private _contactTaskMapsServiceProxy: ContactTaskMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getContactTaskMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._contactTaskMapsServiceProxy
            .getAll(
                this.filterText,
                this.contactFullNameFilter,
                this.taskEventNameFilter,
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

    createContactTaskMap(): void {
        this.createOrEditContactTaskMapModal.show();
    }

    deleteContactTaskMap(contactTaskMap: ContactTaskMapDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._contactTaskMapsServiceProxy.delete(contactTaskMap.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._contactTaskMapsServiceProxy
            .getContactTaskMapsToExcel(this.filterText, this.contactFullNameFilter, this.taskEventNameFilter)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.contactFullNameFilter = '';
        this.taskEventNameFilter = '';

        this.getContactTaskMaps();
    }
}
