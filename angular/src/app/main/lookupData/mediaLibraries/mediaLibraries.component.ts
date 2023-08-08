import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MediaLibrariesServiceProxy, MediaLibraryDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditMediaLibraryModalComponent } from './create-or-edit-mediaLibrary-modal.component';

import { ViewMediaLibraryModalComponent } from './view-mediaLibrary-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DomSanitizer } from '@angular/platform-browser';
import { DecimalPipe } from '@angular/common';
import { CreateOrEditBulkMediaLibraryModalComponent } from './create-or-edit-bulk-media-library-modal/create-or-edit-bulk-media-library-modal.component';

@Component({
    templateUrl: './mediaLibraries.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
    providers: [DecimalPipe],
})
export class MediaLibrariesComponent extends AppComponentBase implements OnInit{
    @ViewChild('createOrEditMediaLibraryModal', { static: true })
    createOrEditMediaLibraryModal: CreateOrEditMediaLibraryModalComponent;
    @ViewChild('viewMediaLibraryModal', { static: true }) viewMediaLibraryModal: ViewMediaLibraryModalComponent;

    @ViewChild('createOrEditModal', { static: true }) createOrEditModal: CreateOrEditBulkMediaLibraryModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    sizeFilter = '';
    fileExtensionFilter = '';
    dimensionFilter = '';
    videoLinkFilter = '';
    seoTagFilter = '';
    altTagFilter = '';
    virtualPathFilter = '';
    binaryObjectIdFilter = '';
    masterTagCategoryNameFilter = '';
    masterTagNameFilter = '';

    skipCount: number;
    maxResultCount: number = 48;

    constructor(
        injector: Injector,
        private _mediaLibrariesServiceProxy: MediaLibrariesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _sanitizer: DomSanitizer,
        private _decimalPipe: DecimalPipe
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.getMediaLibraries();
    }

    getMediaLibraries(event?: LazyLoadEvent) {
        // if (this.primengTableHelper.shouldResetPaging(event)) {
        //     this.paginator.changePage(0);
        //     if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
        //         return;
        //     }
        // }

        this.primengTableHelper.showLoadingIndicator();

        this._mediaLibrariesServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.sizeFilter,
                this.fileExtensionFilter,
                this.dimensionFilter,
                this.videoLinkFilter,
                this.seoTagFilter,
                this.altTagFilter,
                this.virtualPathFilter,
                this.binaryObjectIdFilter,
                this.masterTagCategoryNameFilter,
                this.masterTagNameFilter,
                '',
                this.skipCount,
                this.maxResultCount
                // this.primengTableHelper.getSorting(this.dataTable),
                // this.primengTableHelper.getSkipCount(this.paginator, event),
                // this.primengTableHelper.getMaxResultCount(this.paginator, event)
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

    createMediaLibrary(): void {
        this.createOrEditMediaLibraryModal.isFromMediaLibraryList = true;
        this.createOrEditMediaLibraryModal.show();
    }
    createBulkMediaUpload(): void {
        this.createOrEditModal.show();
    }

    deleteMediaLibrary(mediaLibrary: MediaLibraryDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._mediaLibrariesServiceProxy.delete(mediaLibrary.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._mediaLibrariesServiceProxy
            .getMediaLibrariesToExcel(
                this.filterText,
                this.nameFilter,
                this.sizeFilter,
                this.fileExtensionFilter,
                this.dimensionFilter,
                this.videoLinkFilter,
                this.seoTagFilter,
                this.altTagFilter,
                this.virtualPathFilter,
                this.binaryObjectIdFilter,
                this.masterTagCategoryNameFilter,
                this.masterTagNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.nameFilter = '';
        this.sizeFilter = '';
        this.fileExtensionFilter = '';
        this.dimensionFilter = '';
        this.videoLinkFilter = '';
        this.seoTagFilter = '';
        this.altTagFilter = '';
        this.virtualPathFilter = '';
        this.binaryObjectIdFilter = '';
        this.masterTagCategoryNameFilter = '';
        this.masterTagNameFilter = '';

        this.getMediaLibraries();
    }

    getSafeEmbeddedVideoUrl(url: string) {
        return this._sanitizer.bypassSecurityTrustResourceUrl(url);
    }

    getSizeFormat(size: string) {
        if (size.includes("bytes")) {
            return size;
        }
        let newSize = +size.replace("kb", "");
        return this._decimalPipe.transform(newSize, '1.2-2') + " kb";
    }

    paginate(event: any) {
        this.skipCount = event.first;
        this.maxResultCount = event.rows;
        this.getMediaLibraries();
    }

}
