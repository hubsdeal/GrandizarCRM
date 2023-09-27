import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { StoreProductBlogsServiceProxy, StoreProductBlogDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreProductBlogModalComponent } from './create-or-edit-storeProductBlog-modal.component';

import { ViewStoreProductBlogModalComponent } from './view-storeProductBlog-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './storeProductBlogs.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class StoreProductBlogsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditStoreProductBlogModal', { static: true }) createOrEditStoreProductBlogModal: CreateOrEditStoreProductBlogModalComponent;
    @ViewChild('viewStoreProductBlogModal', { static: true }) viewStoreProductBlogModal: ViewStoreProductBlogModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxDisplaySequenceFilter : number;
		maxDisplaySequenceFilterEmpty : number;
		minDisplaySequenceFilter : number;
		minDisplaySequenceFilterEmpty : number;
        contentTitleFilter = '';
        productNameFilter = '';
        storeNameFilter = '';
        hubNameFilter = '';






    constructor(
        injector: Injector,
        private _storeProductBlogsServiceProxy: StoreProductBlogsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getStoreProductBlogs(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeProductBlogsServiceProxy.getAll(
            this.filterText,
            this.maxDisplaySequenceFilter == null ? this.maxDisplaySequenceFilterEmpty: this.maxDisplaySequenceFilter,
            this.minDisplaySequenceFilter == null ? this.minDisplaySequenceFilterEmpty: this.minDisplaySequenceFilter,
            this.contentTitleFilter,
            this.productNameFilter,
            this.storeNameFilter,
            this.hubNameFilter,
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

    createStoreProductBlog(): void {
        this.createOrEditStoreProductBlogModal.show();        
    }


    deleteStoreProductBlog(storeProductBlog: StoreProductBlogDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._storeProductBlogsServiceProxy.delete(storeProductBlog.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._storeProductBlogsServiceProxy.getStoreProductBlogsToExcel(
        this.filterText,
            this.maxDisplaySequenceFilter == null ? this.maxDisplaySequenceFilterEmpty: this.maxDisplaySequenceFilter,
            this.minDisplaySequenceFilter == null ? this.minDisplaySequenceFilterEmpty: this.minDisplaySequenceFilter,
            this.contentTitleFilter,
            this.productNameFilter,
            this.storeNameFilter,
            this.hubNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.maxDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
		this.minDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
		this.contentTitleFilter = '';
							this.productNameFilter = '';
							this.storeNameFilter = '';
							this.hubNameFilter = '';
					
        this.getStoreProductBlogs();
    }
}
