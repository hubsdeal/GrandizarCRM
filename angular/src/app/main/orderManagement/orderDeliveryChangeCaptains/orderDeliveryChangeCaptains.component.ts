import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { OrderDeliveryChangeCaptainsServiceProxy, OrderDeliveryChangeCaptainDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditOrderDeliveryChangeCaptainModalComponent } from './create-or-edit-orderDeliveryChangeCaptain-modal.component';

import { ViewOrderDeliveryChangeCaptainModalComponent } from './view-orderDeliveryChangeCaptain-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './orderDeliveryChangeCaptains.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class OrderDeliveryChangeCaptainsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditOrderDeliveryChangeCaptainModal', { static: true }) createOrEditOrderDeliveryChangeCaptainModal: CreateOrEditOrderDeliveryChangeCaptainModalComponent;
    @ViewChild('viewOrderDeliveryChangeCaptainModal', { static: true }) viewOrderDeliveryChangeCaptainModal: ViewOrderDeliveryChangeCaptainModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxAcceptedOrderDateTimeFilter : DateTime;
		minAcceptedOrderDateTimeFilter : DateTime;
    maxRejectedOrderDateTimeFilter : DateTime;
		minRejectedOrderDateTimeFilter : DateTime;
    reasonForChangeCaptainFilter = '';
        orderDeliveryByCaptainOrderDeliveryRouteDataFilter = '';
        employeeNameFilter = '';
        employeeName2Filter = '';






    constructor(
        injector: Injector,
        private _orderDeliveryChangeCaptainsServiceProxy: OrderDeliveryChangeCaptainsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getOrderDeliveryChangeCaptains(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._orderDeliveryChangeCaptainsServiceProxy.getAll(
            this.filterText,
            this.maxAcceptedOrderDateTimeFilter === undefined ? this.maxAcceptedOrderDateTimeFilter : this._dateTimeService.getEndOfDayForDate(this.maxAcceptedOrderDateTimeFilter),
            this.minAcceptedOrderDateTimeFilter === undefined ? this.minAcceptedOrderDateTimeFilter : this._dateTimeService.getStartOfDayForDate(this.minAcceptedOrderDateTimeFilter),
            this.maxRejectedOrderDateTimeFilter === undefined ? this.maxRejectedOrderDateTimeFilter : this._dateTimeService.getEndOfDayForDate(this.maxRejectedOrderDateTimeFilter),
            this.minRejectedOrderDateTimeFilter === undefined ? this.minRejectedOrderDateTimeFilter : this._dateTimeService.getStartOfDayForDate(this.minRejectedOrderDateTimeFilter),
            this.reasonForChangeCaptainFilter,
            this.orderDeliveryByCaptainOrderDeliveryRouteDataFilter,
            this.employeeNameFilter,
            this.employeeName2Filter,
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

    createOrderDeliveryChangeCaptain(): void {
        this.createOrEditOrderDeliveryChangeCaptainModal.show();        
    }


    deleteOrderDeliveryChangeCaptain(orderDeliveryChangeCaptain: OrderDeliveryChangeCaptainDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._orderDeliveryChangeCaptainsServiceProxy.delete(orderDeliveryChangeCaptain.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._orderDeliveryChangeCaptainsServiceProxy.getOrderDeliveryChangeCaptainsToExcel(
        this.filterText,
            this.maxAcceptedOrderDateTimeFilter === undefined ? this.maxAcceptedOrderDateTimeFilter : this._dateTimeService.getEndOfDayForDate(this.maxAcceptedOrderDateTimeFilter),
            this.minAcceptedOrderDateTimeFilter === undefined ? this.minAcceptedOrderDateTimeFilter : this._dateTimeService.getStartOfDayForDate(this.minAcceptedOrderDateTimeFilter),
            this.maxRejectedOrderDateTimeFilter === undefined ? this.maxRejectedOrderDateTimeFilter : this._dateTimeService.getEndOfDayForDate(this.maxRejectedOrderDateTimeFilter),
            this.minRejectedOrderDateTimeFilter === undefined ? this.minRejectedOrderDateTimeFilter : this._dateTimeService.getStartOfDayForDate(this.minRejectedOrderDateTimeFilter),
            this.reasonForChangeCaptainFilter,
            this.orderDeliveryByCaptainOrderDeliveryRouteDataFilter,
            this.employeeNameFilter,
            this.employeeName2Filter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.maxAcceptedOrderDateTimeFilter = undefined;
		this.minAcceptedOrderDateTimeFilter = undefined;
    this.maxRejectedOrderDateTimeFilter = undefined;
		this.minRejectedOrderDateTimeFilter = undefined;
    this.reasonForChangeCaptainFilter = '';
		this.orderDeliveryByCaptainOrderDeliveryRouteDataFilter = '';
							this.employeeNameFilter = '';
							this.employeeName2Filter = '';
					
        this.getOrderDeliveryChangeCaptains();
    }
}
