import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderSalesChannelsServiceProxy, OrderSalesChannelDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditOrderSalesChannelModalComponent } from './create-or-edit-orderSalesChannel-modal.component';

import { ViewOrderSalesChannelModalComponent } from './view-orderSalesChannel-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './orderSalesChannels.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class OrderSalesChannelsComponent extends AppComponentBase {
    @ViewChild('createOrEditOrderSalesChannelModal', { static: true })
    createOrEditOrderSalesChannelModal: CreateOrEditOrderSalesChannelModalComponent;
    @ViewChild('viewOrderSalesChannelModal', { static: true })
    viewOrderSalesChannelModal: ViewOrderSalesChannelModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    linkNameFilter = '';
    apiLinkFilter = '';
    userIdFilter = '';
    passwordFilter = '';
    notesFilter = '';

    constructor(
        injector: Injector,
        private _orderSalesChannelsServiceProxy: OrderSalesChannelsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getOrderSalesChannels(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._orderSalesChannelsServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.linkNameFilter,
                this.apiLinkFilter,
                this.userIdFilter,
                this.passwordFilter,
                this.notesFilter,
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

    createOrderSalesChannel(): void {
        this.createOrEditOrderSalesChannelModal.show();
    }

    deleteOrderSalesChannel(orderSalesChannel: OrderSalesChannelDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._orderSalesChannelsServiceProxy.delete(orderSalesChannel.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._orderSalesChannelsServiceProxy
            .getOrderSalesChannelsToExcel(
                this.filterText,
                this.nameFilter,
                this.linkNameFilter,
                this.apiLinkFilter,
                this.userIdFilter,
                this.passwordFilter,
                this.notesFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.nameFilter = '';
        this.linkNameFilter = '';
        this.apiLinkFilter = '';
        this.userIdFilter = '';
        this.passwordFilter = '';
        this.notesFilter = '';

        this.getOrderSalesChannels();
    }
}
