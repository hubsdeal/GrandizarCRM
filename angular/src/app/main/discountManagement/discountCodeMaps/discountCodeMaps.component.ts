import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DiscountCodeMapsServiceProxy, DiscountCodeMapDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditDiscountCodeMapModalComponent } from './create-or-edit-discountCodeMap-modal.component';

import { ViewDiscountCodeMapModalComponent } from './view-discountCodeMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './discountCodeMaps.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class DiscountCodeMapsComponent extends AppComponentBase {
    @ViewChild('createOrEditDiscountCodeMapModal', { static: true })
    createOrEditDiscountCodeMapModal: CreateOrEditDiscountCodeMapModalComponent;
    @ViewChild('viewDiscountCodeMapModal', { static: true })
    viewDiscountCodeMapModal: ViewDiscountCodeMapModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    discountCodeGeneratorNameFilter = '';
    storeNameFilter = '';
    productNameFilter = '';
    membershipTypeNameFilter = '';

    constructor(
        injector: Injector,
        private _discountCodeMapsServiceProxy: DiscountCodeMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getDiscountCodeMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._discountCodeMapsServiceProxy
            .getAll(
                this.filterText,
                this.discountCodeGeneratorNameFilter,
                this.storeNameFilter,
                this.productNameFilter,
                this.membershipTypeNameFilter,
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

    createDiscountCodeMap(): void {
        this.createOrEditDiscountCodeMapModal.show();
    }

    deleteDiscountCodeMap(discountCodeMap: DiscountCodeMapDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._discountCodeMapsServiceProxy.delete(discountCodeMap.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._discountCodeMapsServiceProxy
            .getDiscountCodeMapsToExcel(
                this.filterText,
                this.discountCodeGeneratorNameFilter,
                this.storeNameFilter,
                this.productNameFilter,
                this.membershipTypeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.discountCodeGeneratorNameFilter = '';
        this.storeNameFilter = '';
        this.productNameFilter = '';
        this.membershipTypeNameFilter = '';

        this.getDiscountCodeMaps();
    }
}
