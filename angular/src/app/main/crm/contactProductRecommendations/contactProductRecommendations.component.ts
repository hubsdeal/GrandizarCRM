import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { ContactProductRecommendationsServiceProxy, ContactProductRecommendationDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditContactProductRecommendationModalComponent } from './create-or-edit-contactProductRecommendation-modal.component';

import { ViewContactProductRecommendationModalComponent } from './view-contactProductRecommendation-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './contactProductRecommendations.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ContactProductRecommendationsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditContactProductRecommendationModal', { static: true }) createOrEditContactProductRecommendationModal: CreateOrEditContactProductRecommendationModalComponent;
    @ViewChild('viewContactProductRecommendationModal', { static: true }) viewContactProductRecommendationModal: ViewContactProductRecommendationModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxHubRecommendationScoreFilter : number;
		maxHubRecommendationScoreFilterEmpty : number;
		minHubRecommendationScoreFilter : number;
		minHubRecommendationScoreFilterEmpty : number;
    maxStoreRecommendationScoreFilter : number;
		maxStoreRecommendationScoreFilterEmpty : number;
		minStoreRecommendationScoreFilter : number;
		minStoreRecommendationScoreFilterEmpty : number;
    maxProductRecommendationScoreFilter : number;
		maxProductRecommendationScoreFilterEmpty : number;
		minProductRecommendationScoreFilter : number;
		minProductRecommendationScoreFilterEmpty : number;
    maxProductCategoryRecommendationScoreFilter : number;
		maxProductCategoryRecommendationScoreFilterEmpty : number;
		minProductCategoryRecommendationScoreFilter : number;
		minProductCategoryRecommendationScoreFilterEmpty : number;
    maxJobRecommendationScoreFilter : number;
		maxJobRecommendationScoreFilterEmpty : number;
		minJobRecommendationScoreFilter : number;
		minJobRecommendationScoreFilterEmpty : number;
        userNameFilter = '';
        contactFullNameFilter = '';
        hubNameFilter = '';
        storeNameFilter = '';
        productNameFilter = '';
        productCategoryNameFilter = '';
        jobTitleFilter = '';






    constructor(
        injector: Injector,
        private _contactProductRecommendationsServiceProxy: ContactProductRecommendationsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getContactProductRecommendations(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._contactProductRecommendationsServiceProxy.getAll(
            this.filterText,
            this.maxHubRecommendationScoreFilter == null ? this.maxHubRecommendationScoreFilterEmpty: this.maxHubRecommendationScoreFilter,
            this.minHubRecommendationScoreFilter == null ? this.minHubRecommendationScoreFilterEmpty: this.minHubRecommendationScoreFilter,
            this.maxStoreRecommendationScoreFilter == null ? this.maxStoreRecommendationScoreFilterEmpty: this.maxStoreRecommendationScoreFilter,
            this.minStoreRecommendationScoreFilter == null ? this.minStoreRecommendationScoreFilterEmpty: this.minStoreRecommendationScoreFilter,
            this.maxProductRecommendationScoreFilter == null ? this.maxProductRecommendationScoreFilterEmpty: this.maxProductRecommendationScoreFilter,
            this.minProductRecommendationScoreFilter == null ? this.minProductRecommendationScoreFilterEmpty: this.minProductRecommendationScoreFilter,
            this.maxProductCategoryRecommendationScoreFilter == null ? this.maxProductCategoryRecommendationScoreFilterEmpty: this.maxProductCategoryRecommendationScoreFilter,
            this.minProductCategoryRecommendationScoreFilter == null ? this.minProductCategoryRecommendationScoreFilterEmpty: this.minProductCategoryRecommendationScoreFilter,
            this.maxJobRecommendationScoreFilter == null ? this.maxJobRecommendationScoreFilterEmpty: this.maxJobRecommendationScoreFilter,
            this.minJobRecommendationScoreFilter == null ? this.minJobRecommendationScoreFilterEmpty: this.minJobRecommendationScoreFilter,
            this.userNameFilter,
            this.contactFullNameFilter,
            this.hubNameFilter,
            this.storeNameFilter,
            this.productNameFilter,
            this.productCategoryNameFilter,
            this.jobTitleFilter,
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

    createContactProductRecommendation(): void {
        this.createOrEditContactProductRecommendationModal.show();        
    }


    deleteContactProductRecommendation(contactProductRecommendation: ContactProductRecommendationDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._contactProductRecommendationsServiceProxy.delete(contactProductRecommendation.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._contactProductRecommendationsServiceProxy.getContactProductRecommendationsToExcel(
        this.filterText,
            this.maxHubRecommendationScoreFilter == null ? this.maxHubRecommendationScoreFilterEmpty: this.maxHubRecommendationScoreFilter,
            this.minHubRecommendationScoreFilter == null ? this.minHubRecommendationScoreFilterEmpty: this.minHubRecommendationScoreFilter,
            this.maxStoreRecommendationScoreFilter == null ? this.maxStoreRecommendationScoreFilterEmpty: this.maxStoreRecommendationScoreFilter,
            this.minStoreRecommendationScoreFilter == null ? this.minStoreRecommendationScoreFilterEmpty: this.minStoreRecommendationScoreFilter,
            this.maxProductRecommendationScoreFilter == null ? this.maxProductRecommendationScoreFilterEmpty: this.maxProductRecommendationScoreFilter,
            this.minProductRecommendationScoreFilter == null ? this.minProductRecommendationScoreFilterEmpty: this.minProductRecommendationScoreFilter,
            this.maxProductCategoryRecommendationScoreFilter == null ? this.maxProductCategoryRecommendationScoreFilterEmpty: this.maxProductCategoryRecommendationScoreFilter,
            this.minProductCategoryRecommendationScoreFilter == null ? this.minProductCategoryRecommendationScoreFilterEmpty: this.minProductCategoryRecommendationScoreFilter,
            this.maxJobRecommendationScoreFilter == null ? this.maxJobRecommendationScoreFilterEmpty: this.maxJobRecommendationScoreFilter,
            this.minJobRecommendationScoreFilter == null ? this.minJobRecommendationScoreFilterEmpty: this.minJobRecommendationScoreFilter,
            this.userNameFilter,
            this.contactFullNameFilter,
            this.hubNameFilter,
            this.storeNameFilter,
            this.productNameFilter,
            this.productCategoryNameFilter,
            this.jobTitleFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.maxHubRecommendationScoreFilter = this.maxHubRecommendationScoreFilterEmpty;
		this.minHubRecommendationScoreFilter = this.maxHubRecommendationScoreFilterEmpty;
    this.maxStoreRecommendationScoreFilter = this.maxStoreRecommendationScoreFilterEmpty;
		this.minStoreRecommendationScoreFilter = this.maxStoreRecommendationScoreFilterEmpty;
    this.maxProductRecommendationScoreFilter = this.maxProductRecommendationScoreFilterEmpty;
		this.minProductRecommendationScoreFilter = this.maxProductRecommendationScoreFilterEmpty;
    this.maxProductCategoryRecommendationScoreFilter = this.maxProductCategoryRecommendationScoreFilterEmpty;
		this.minProductCategoryRecommendationScoreFilter = this.maxProductCategoryRecommendationScoreFilterEmpty;
    this.maxJobRecommendationScoreFilter = this.maxJobRecommendationScoreFilterEmpty;
		this.minJobRecommendationScoreFilter = this.maxJobRecommendationScoreFilterEmpty;
		this.userNameFilter = '';
							this.contactFullNameFilter = '';
							this.hubNameFilter = '';
							this.storeNameFilter = '';
							this.productNameFilter = '';
							this.productCategoryNameFilter = '';
							this.jobTitleFilter = '';
					
        this.getContactProductRecommendations();
    }
}
