import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SubscriptionTypesServiceProxy, SubscriptionTypeDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditSubscriptionTypeModalComponent } from './create-or-edit-subscriptionType-modal.component';

import { ViewSubscriptionTypeModalComponent } from './view-subscriptionType-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './subscriptionTypes.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class SubscriptionTypesComponent extends AppComponentBase {
    @ViewChild('createOrEditSubscriptionTypeModal', { static: true })
    createOrEditSubscriptionTypeModal: CreateOrEditSubscriptionTypeModalComponent;
    @ViewChild('viewSubscriptionTypeModal', { static: true })
    viewSubscriptionTypeModal: ViewSubscriptionTypeModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    maxNumberOfDaysFilter: number;
    maxNumberOfDaysFilterEmpty: number;
    minNumberOfDaysFilter: number;
    minNumberOfDaysFilterEmpty: number;

    constructor(
        injector: Injector,
        private _subscriptionTypesServiceProxy: SubscriptionTypesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getSubscriptionTypes(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._subscriptionTypesServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.maxNumberOfDaysFilter == null ? this.maxNumberOfDaysFilterEmpty : this.maxNumberOfDaysFilter,
                this.minNumberOfDaysFilter == null ? this.minNumberOfDaysFilterEmpty : this.minNumberOfDaysFilter,
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

    createSubscriptionType(): void {
        this.createOrEditSubscriptionTypeModal.show();
    }

    deleteSubscriptionType(subscriptionType: SubscriptionTypeDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._subscriptionTypesServiceProxy.delete(subscriptionType.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._subscriptionTypesServiceProxy
            .getSubscriptionTypesToExcel(
                this.filterText,
                this.nameFilter,
                this.maxNumberOfDaysFilter == null ? this.maxNumberOfDaysFilterEmpty : this.maxNumberOfDaysFilter,
                this.minNumberOfDaysFilter == null ? this.minNumberOfDaysFilterEmpty : this.minNumberOfDaysFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.nameFilter = '';
        this.maxNumberOfDaysFilter = this.maxNumberOfDaysFilterEmpty;
        this.minNumberOfDaysFilter = this.maxNumberOfDaysFilterEmpty;

        this.getSubscriptionTypes();
    }
}
