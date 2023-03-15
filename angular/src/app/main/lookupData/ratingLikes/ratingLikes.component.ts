import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RatingLikesServiceProxy, RatingLikeDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditRatingLikeModalComponent } from './create-or-edit-ratingLike-modal.component';

import { ViewRatingLikeModalComponent } from './view-ratingLike-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './ratingLikes.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class RatingLikesComponent extends AppComponentBase {
    @ViewChild('createOrEditRatingLikeModal', { static: true })
    createOrEditRatingLikeModal: CreateOrEditRatingLikeModalComponent;
    @ViewChild('viewRatingLikeModal', { static: true }) viewRatingLikeModal: ViewRatingLikeModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    maxScoreFilter: number;
    maxScoreFilterEmpty: number;
    minScoreFilter: number;
    minScoreFilterEmpty: number;
    iconLinkFilter = '';

    constructor(
        injector: Injector,
        private _ratingLikesServiceProxy: RatingLikesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getRatingLikes(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._ratingLikesServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.maxScoreFilter == null ? this.maxScoreFilterEmpty : this.maxScoreFilter,
                this.minScoreFilter == null ? this.minScoreFilterEmpty : this.minScoreFilter,
                this.iconLinkFilter,
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

    createRatingLike(): void {
        this.createOrEditRatingLikeModal.show();
    }

    deleteRatingLike(ratingLike: RatingLikeDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._ratingLikesServiceProxy.delete(ratingLike.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._ratingLikesServiceProxy
            .getRatingLikesToExcel(
                this.filterText,
                this.nameFilter,
                this.maxScoreFilter == null ? this.maxScoreFilterEmpty : this.maxScoreFilter,
                this.minScoreFilter == null ? this.minScoreFilterEmpty : this.minScoreFilter,
                this.iconLinkFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.nameFilter = '';
        this.maxScoreFilter = this.maxScoreFilterEmpty;
        this.minScoreFilter = this.maxScoreFilterEmpty;
        this.iconLinkFilter = '';

        this.getRatingLikes();
    }
}
