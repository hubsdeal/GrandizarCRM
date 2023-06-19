import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductTagsServiceProxy, ProductTagDto, MasterTagCategoryForDashboardViewDto, MasterTagForDashboardViewDto, CreateOrEditProductTagDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductTagModalComponent } from './create-or-edit-productTag-modal.component';

import { ViewProductTagModalComponent } from './view-productTag-modal.component';
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
    selector: 'app-productTags',
    templateUrl: './productTags.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductTagsComponent extends AppComponentBase {
    @ViewChild('createOrEditProductTagModal', { static: true })
    createOrEditProductTagModal: CreateOrEditProductTagModalComponent;
    @ViewChild('viewProductTagModal', { static: true }) viewProductTagModal: ViewProductTagModalComponent;

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
    productNameFilter = '';
    masterTagCategoryNameFilter = '';
    masterTagNameFilter = '';

    @Input() productId: number;
    @Input() productCategoryId: number;

    allProductTags: MasterTagCategoryForDashboardViewDto[] = [];
    showMoreAndShowLess = false;

    loader_anim_show: boolean = false;
    loaderSelectedMasterTagId: number;

    constructor(
        injector: Injector,
        private _productTagsServiceProxy: ProductTagsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getProductTags(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productTagsServiceProxy
            .getAll(
                this.filterText,
                this.customTagFilter,
                this.tagValueFilter,
                this.verifiedFilter,
                this.maxSequenceFilter == null ? this.maxSequenceFilterEmpty : this.maxSequenceFilter,
                this.minSequenceFilter == null ? this.minSequenceFilterEmpty : this.minSequenceFilter,
                this.productNameFilter,
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

    createProductTag(): void {
        this.createOrEditProductTagModal.show();
    }

    deleteProductTag(productTag: ProductTagDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._productTagsServiceProxy.delete(productTag.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._productTagsServiceProxy
            .getProductTagsToExcel(
                this.filterText,
                this.customTagFilter,
                this.tagValueFilter,
                this.verifiedFilter,
                this.maxSequenceFilter == null ? this.maxSequenceFilterEmpty : this.maxSequenceFilter,
                this.minSequenceFilter == null ? this.minSequenceFilterEmpty : this.minSequenceFilter,
                this.productNameFilter,
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
        this.productNameFilter = '';
        this.masterTagCategoryNameFilter = '';
        this.masterTagNameFilter = '';

        this.getProductTags();
    }

    getALlProductTags() {
        this._productTagsServiceProxy.getProductTagsByTagSetting(this.productCategoryId, this.productId).subscribe((result) => {
            this.allProductTags = result;
            this.loader_anim_show = false;
        });
    }

    onTagSelect(event: MasterTagForDashboardViewDto) {
        this.loader_anim_show = true;
        this.loaderSelectedMasterTagId = event.id;
        if (event.isSelected) {
            this._productTagsServiceProxy.deleteByProductAndTag(this.productId, event.id).subscribe((result) => {
                this.getALlProductTags();
            });
        } else {
            var tag = new CreateOrEditProductTagDto();
            tag.productId = this.productId;
            tag.masterTagCategoryId = event.masterTagCategoryId;
            tag.masterTagId = event.id;
            this._productTagsServiceProxy.createOrEdit(tag).subscribe(result => {
                this.getALlProductTags();
            });
        }
    }

    drop(event: CdkDragDrop<string[]>) {
        console.log(this.allProductTags, event.previousIndex, event.currentIndex);
        moveItemInArray(this.allProductTags, event.previousIndex, event.currentIndex);
    }

    ngOnInit(): void {
        this.getALlProductTags();
    }
}
