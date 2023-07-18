import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HubNavigationMenusServiceProxy, HubNavigationMenuDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditHubNavigationMenuModalComponent } from './create-or-edit-hubNavigationMenu-modal.component';

import { ViewHubNavigationMenuModalComponent } from './view-hubNavigationMenu-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'app-hubNavigationMenus',
    templateUrl: './hubNavigationMenus.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class HubNavigationMenusComponent extends AppComponentBase {
    @ViewChild('createOrEditHubNavigationMenuModal', { static: true })
    createOrEditHubNavigationMenuModal: CreateOrEditHubNavigationMenuModalComponent;
    @ViewChild('viewHubNavigationMenuModal', { static: true })
    viewHubNavigationMenuModal: ViewHubNavigationMenuModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    customNameFilter = '';
    navigationLinkFilter = '';
    hubNameFilter = '';
    masterNavigationMenuNameFilter = '';

    constructor(
        injector: Injector,
        private _hubNavigationMenusServiceProxy: HubNavigationMenusServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getHubNavigationMenus(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._hubNavigationMenusServiceProxy
            .getAll(
                this.filterText,
                this.customNameFilter,
                this.navigationLinkFilter,
                this.hubNameFilter,
                this.masterNavigationMenuNameFilter,
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

    createHubNavigationMenu(): void {
        this.createOrEditHubNavigationMenuModal.show();
    }

    deleteHubNavigationMenu(hubNavigationMenu: HubNavigationMenuDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._hubNavigationMenusServiceProxy.delete(hubNavigationMenu.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._hubNavigationMenusServiceProxy
            .getHubNavigationMenusToExcel(
                this.filterText,
                this.customNameFilter,
                this.navigationLinkFilter,
                this.hubNameFilter,
                this.masterNavigationMenuNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.customNameFilter = '';
        this.navigationLinkFilter = '';
        this.hubNameFilter = '';
        this.masterNavigationMenuNameFilter = '';

        this.getHubNavigationMenus();
    }
}
