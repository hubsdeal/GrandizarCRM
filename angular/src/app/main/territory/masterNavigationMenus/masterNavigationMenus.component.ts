import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MasterNavigationMenusServiceProxy, MasterNavigationMenuDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditMasterNavigationMenuModalComponent } from './create-or-edit-masterNavigationMenu-modal.component';

import { ViewMasterNavigationMenuModalComponent } from './view-masterNavigationMenu-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './masterNavigationMenus.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class MasterNavigationMenusComponent extends AppComponentBase {
    @ViewChild('createOrEditMasterNavigationMenuModal', { static: true })
    createOrEditMasterNavigationMenuModal: CreateOrEditMasterNavigationMenuModalComponent;
    @ViewChild('viewMasterNavigationMenuModal', { static: true })
    viewMasterNavigationMenuModal: ViewMasterNavigationMenuModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    hasParentMenuFilter = -1;
    maxParentMenuIdFilter: number;
    maxParentMenuIdFilterEmpty: number;
    minParentMenuIdFilter: number;
    minParentMenuIdFilterEmpty: number;
    iconLinkFilter = '';
    mediaLinkFilter = '';

    constructor(
        injector: Injector,
        private _masterNavigationMenusServiceProxy: MasterNavigationMenusServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getMasterNavigationMenus(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._masterNavigationMenusServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.hasParentMenuFilter,
                this.maxParentMenuIdFilter == null ? this.maxParentMenuIdFilterEmpty : this.maxParentMenuIdFilter,
                this.minParentMenuIdFilter == null ? this.minParentMenuIdFilterEmpty : this.minParentMenuIdFilter,
                this.iconLinkFilter,
                this.mediaLinkFilter,
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

    createMasterNavigationMenu(): void {
        this.createOrEditMasterNavigationMenuModal.show();
    }

    deleteMasterNavigationMenu(masterNavigationMenu: MasterNavigationMenuDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._masterNavigationMenusServiceProxy.delete(masterNavigationMenu.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._masterNavigationMenusServiceProxy
            .getMasterNavigationMenusToExcel(
                this.filterText,
                this.nameFilter,
                this.hasParentMenuFilter,
                this.maxParentMenuIdFilter == null ? this.maxParentMenuIdFilterEmpty : this.maxParentMenuIdFilter,
                this.minParentMenuIdFilter == null ? this.minParentMenuIdFilterEmpty : this.minParentMenuIdFilter,
                this.iconLinkFilter,
                this.mediaLinkFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.nameFilter = '';
        this.hasParentMenuFilter = -1;
        this.maxParentMenuIdFilter = this.maxParentMenuIdFilterEmpty;
        this.minParentMenuIdFilter = this.maxParentMenuIdFilterEmpty;
        this.iconLinkFilter = '';
        this.mediaLinkFilter = '';

        this.getMasterNavigationMenus();
    }
}
