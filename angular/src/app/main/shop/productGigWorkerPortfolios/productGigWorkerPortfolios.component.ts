import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { ProductGigWorkerPortfoliosServiceProxy, ProductGigWorkerPortfolioDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductGigWorkerPortfolioModalComponent } from './create-or-edit-productGigWorkerPortfolio-modal.component';

import { ViewProductGigWorkerPortfolioModalComponent } from './view-productGigWorkerPortfolio-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './productGigWorkerPortfolios.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ProductGigWorkerPortfoliosComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditProductGigWorkerPortfolioModal', { static: true }) createOrEditProductGigWorkerPortfolioModal: CreateOrEditProductGigWorkerPortfolioModalComponent;
    @ViewChild('viewProductGigWorkerPortfolioModal', { static: true }) viewProductGigWorkerPortfolioModal: ViewProductGigWorkerPortfolioModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxDisplaySequenceFilter : number;
		maxDisplaySequenceFilterEmpty : number;
		minDisplaySequenceFilter : number;
		minDisplaySequenceFilterEmpty : number;
        businessNameFilter = '';
        contactFullNameFilter = '';
        productNameFilter = '';
        employeeNameFilter = '';






    constructor(
        injector: Injector,
        private _productGigWorkerPortfoliosServiceProxy: ProductGigWorkerPortfoliosServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getProductGigWorkerPortfolios(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productGigWorkerPortfoliosServiceProxy.getAll(
            this.filterText,
            this.maxDisplaySequenceFilter == null ? this.maxDisplaySequenceFilterEmpty: this.maxDisplaySequenceFilter,
            this.minDisplaySequenceFilter == null ? this.minDisplaySequenceFilterEmpty: this.minDisplaySequenceFilter,
            this.businessNameFilter,
            this.contactFullNameFilter,
            this.productNameFilter,
            this.employeeNameFilter,
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

    createProductGigWorkerPortfolio(): void {
        this.createOrEditProductGigWorkerPortfolioModal.show();        
    }


    deleteProductGigWorkerPortfolio(productGigWorkerPortfolio: ProductGigWorkerPortfolioDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._productGigWorkerPortfoliosServiceProxy.delete(productGigWorkerPortfolio.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._productGigWorkerPortfoliosServiceProxy.getProductGigWorkerPortfoliosToExcel(
        this.filterText,
            this.maxDisplaySequenceFilter == null ? this.maxDisplaySequenceFilterEmpty: this.maxDisplaySequenceFilter,
            this.minDisplaySequenceFilter == null ? this.minDisplaySequenceFilterEmpty: this.minDisplaySequenceFilter,
            this.businessNameFilter,
            this.contactFullNameFilter,
            this.productNameFilter,
            this.employeeNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.maxDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
		this.minDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
		this.businessNameFilter = '';
							this.contactFullNameFilter = '';
							this.productNameFilter = '';
							this.employeeNameFilter = '';
					
        this.getProductGigWorkerPortfolios();
    }
}
