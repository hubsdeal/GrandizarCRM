import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductCategoryTeamsServiceProxy, ProductCategoryTeamDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductCategoryTeamModalComponent } from './create-or-edit-productCategoryTeam-modal.component';

import { ViewProductCategoryTeamModalComponent } from './view-productCategoryTeam-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter, trim } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { OneToOneConnectModalComponent } from '@app/shared/one-to-one-connect-modal/one-to-one-connect-modal.component';

@Component({
    selector: 'appProductCategoryTeams',
    templateUrl: './productCategoryTeams.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductCategoryTeamsComponent extends AppComponentBase implements OnInit {


    @ViewChild('createOrEditProductCategoryTeamModal', { static: true }) createOrEditProductCategoryTeamModal: CreateOrEditProductCategoryTeamModalComponent;
    @ViewChild('viewProductCategoryTeamModalComponent', { static: true }) viewProductCategoryTeamModal: ViewProductCategoryTeamModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('oneToOneConnectModal', { static: true }) oneToOneConnectModal: OneToOneConnectModalComponent;
    advancedFiltersAreShown = false;
    filterText = '';
    productCategoryNameFilter = '';
    employeeNameFilter = '';

    @Input() productCategoryId: number;




    constructor(
        injector: Injector,
        private _productCategoryTeamsServiceProxy: ProductCategoryTeamsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    ngOnInit(): void{
        this.getProductCategoryTeams();
    }

    getProductCategoryTeams(event?: LazyLoadEvent) {
        // if (this.primengTableHelper.shouldResetPaging(event)) {
        //     this.paginator.changePage(0);
        //     return;
        // }

        this.primengTableHelper.showLoadingIndicator();

        // this._productCategoryTeamsServiceProxy.getAll(
        //     this.filterText,
        //     this.productCategoryNameFilter,
        //     this.employeeNameFilter,
        //     this.primengTableHelper.getSorting(this.dataTable),
        //     this.primengTableHelper.getSkipCount(this.paginator, event),
        //     this.primengTableHelper.getMaxResultCount(this.paginator, event)
        // ).subscribe(result => {
        //     this.primengTableHelper.totalRecordsCount = result.totalCount;
        //     this.primengTableHelper.records = result.items;
        //     this.primengTableHelper.hideLoadingIndicator();
        // });

        this._productCategoryTeamsServiceProxy.getAllByProductCategoryId(
            this.productCategoryId
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.getProductCategoryTeams();
        // this.paginator.changePage(this.paginator.getPage());
    }

    createProductCategoryTeam(): void {
        this.createOrEditProductCategoryTeamModal.productCategoryId = this.productCategoryId;
        this.createOrEditProductCategoryTeamModal.show();
    }


    deleteProductCategoryTeam(productCategoryTeam: ProductCategoryTeamDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._productCategoryTeamsServiceProxy.delete(productCategoryTeam.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._productCategoryTeamsServiceProxy.getProductCategoryTeamsToExcel(
            this.filterText,
            null,
            this.productCategoryNameFilter,
            this.employeeNameFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    onConnectClick(id: number) {
        this.oneToOneConnectModal.employeeId = id;
        this.oneToOneConnectModal.show();
    }

    splitTeamName(name: string): string {
        name = trim(name);
        var splitNames = name.split(" ");
        let characters = "";
        for (let i = 0; i < splitNames.length; i++) {
            if (i > 1) {
                break;
            }
            characters += splitNames[i][0];
        }
        return characters;
    }

    getTotalcount(total){
        return '| Total : ' + total;
    }

}
