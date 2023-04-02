import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderStatusesServiceProxy, OrderStatusDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditOrderStatusModalComponent } from './create-or-edit-orderStatus-modal.component';

import { ViewOrderStatusModalComponent } from './view-orderStatus-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './orderStatuses.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class OrderStatusesComponent extends AppComponentBase {
    @ViewChild('createOrEditOrderStatusModal', { static: true })
    createOrEditOrderStatusModal: CreateOrEditOrderStatusModalComponent;
    @ViewChild('viewOrderStatusModal', { static: true }) viewOrderStatusModal: ViewOrderStatusModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    descriptionFilter = '';
    maxSequenceNoFilter: number;
    maxSequenceNoFilterEmpty: number;
    minSequenceNoFilter: number;
    minSequenceNoFilterEmpty: number;
    colorCodeFilter = '';
    messageFilter = '';
    deliveryOrPickupFilter = -1;
    roleNameFilter = '';

    constructor(
        injector: Injector,
        private _orderStatusesServiceProxy: OrderStatusesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getOrderStatuses(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._orderStatusesServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.descriptionFilter,
                this.maxSequenceNoFilter == null ? this.maxSequenceNoFilterEmpty : this.maxSequenceNoFilter,
                this.minSequenceNoFilter == null ? this.minSequenceNoFilterEmpty : this.minSequenceNoFilter,
                this.colorCodeFilter,
                this.messageFilter,
                this.deliveryOrPickupFilter,
                this.roleNameFilter,
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

    createOrderStatus(): void {
        this.createOrEditOrderStatusModal.show();
    }

    deleteOrderStatus(orderStatus: OrderStatusDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._orderStatusesServiceProxy.delete(orderStatus.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._orderStatusesServiceProxy
            .getOrderStatusesToExcel(
                this.filterText,
                this.nameFilter,
                this.descriptionFilter,
                this.maxSequenceNoFilter == null ? this.maxSequenceNoFilterEmpty : this.maxSequenceNoFilter,
                this.minSequenceNoFilter == null ? this.minSequenceNoFilterEmpty : this.minSequenceNoFilter,
                this.colorCodeFilter,
                this.messageFilter,
                this.deliveryOrPickupFilter,
                this.roleNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.nameFilter = '';
        this.descriptionFilter = '';
        this.maxSequenceNoFilter = this.maxSequenceNoFilterEmpty;
        this.minSequenceNoFilter = this.maxSequenceNoFilterEmpty;
        this.colorCodeFilter = '';
        this.messageFilter = '';
        this.deliveryOrPickupFilter = -1;
        this.roleNameFilter = '';

        this.getOrderStatuses();
    }
}
