import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StoreTagsServiceProxy, StoreTagDto, MasterTagCategoryForDashboardViewDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreTagModalComponent } from './create-or-edit-storeTag-modal.component';

import { ViewStoreTagModalComponent } from './view-storeTag-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

@Component({
    selector: 'appStoreTags',
    templateUrl: './storeTags.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoreTagsComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditStoreTagModal', { static: true })
    createOrEditStoreTagModal: CreateOrEditStoreTagModalComponent;
    @ViewChild('viewStoreTagModal', { static: true }) viewStoreTagModal: ViewStoreTagModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    customTagFilter = '';
    tagValueFilter = '';
    verifiedFilter = -1;
    maxSequenceFilter: number;
    maxSequenceFilterEmpty: number;
    minSequenceFilter: number;
    minSequenceFilterEmpty: number;
    storeNameFilter = '';
    masterTagCategoryNameFilter = '';
    masterTagNameFilter = '';

    @Input() storeId: number;
    @Input() storeTagSettingCategoryId: number;

    allStoreTags: MasterTagCategoryForDashboardViewDto[] = [];
    showMoreAndShowLess = false;

    storeTags = [
        {
            id: "1",
            value: "Sales Status"
        },

        {
            id: "2",
            value: "Delivery Types"
        },
        {
            id: "3",
            value: "Customer Group"
        }
    ];

    constructor(
        injector: Injector,
        private _storeTagsServiceProxy: StoreTagsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getStoreTags(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeTagsServiceProxy
            .getAll(
                this.filterText,
                this.customTagFilter,
                this.tagValueFilter,
                this.verifiedFilter,
                this.maxSequenceFilter == null ? this.maxSequenceFilterEmpty : this.maxSequenceFilter,
                this.minSequenceFilter == null ? this.minSequenceFilterEmpty : this.minSequenceFilter,
                this.storeNameFilter,
                this.masterTagCategoryNameFilter,
                this.masterTagNameFilter,
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

    createStoreTag(): void {
        this.createOrEditStoreTagModal.show();
    }

    deleteStoreTag(storeTag: StoreTagDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._storeTagsServiceProxy.delete(storeTag.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._storeTagsServiceProxy
            .getStoreTagsToExcel(
                this.filterText,
                this.customTagFilter,
                this.tagValueFilter,
                this.verifiedFilter,
                this.maxSequenceFilter == null ? this.maxSequenceFilterEmpty : this.maxSequenceFilter,
                this.minSequenceFilter == null ? this.minSequenceFilterEmpty : this.minSequenceFilter,
                this.storeNameFilter,
                this.masterTagCategoryNameFilter,
                this.masterTagNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.customTagFilter = '';
        this.tagValueFilter = '';
        this.verifiedFilter = -1;
        this.maxSequenceFilter = this.maxSequenceFilterEmpty;
        this.minSequenceFilter = this.maxSequenceFilterEmpty;
        this.storeNameFilter = '';
        this.masterTagCategoryNameFilter = '';
        this.masterTagNameFilter = '';

        this.getStoreTags();
    }
    getALlStoreTags() {
        this._storeTagsServiceProxy.getStoreTagsByTagSetting(this.storeTagSettingCategoryId, this.storeId).subscribe((result) => {
            this.allStoreTags = result;
        });
    }

    drop(event: CdkDragDrop<string[]>) {
        console.log(this.allStoreTags, event.previousIndex, event.currentIndex);
        moveItemInArray(this.allStoreTags, event.previousIndex, event.currentIndex);
    }

    ngOnInit(): void {
        this.getALlStoreTags();
        console.log(this.storeId);
        console.log(this.storeTagSettingCategoryId);
    }

}
