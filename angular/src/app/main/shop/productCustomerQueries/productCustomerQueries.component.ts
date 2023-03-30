import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductCustomerQueriesServiceProxy, ProductCustomerQueryDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductCustomerQueryModalComponent } from './create-or-edit-productCustomerQuery-modal.component';

import { ViewProductCustomerQueryModalComponent } from './view-productCustomerQuery-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './productCustomerQueries.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductCustomerQueriesComponent extends AppComponentBase {
    @ViewChild('createOrEditProductCustomerQueryModal', { static: true })
    createOrEditProductCustomerQueryModal: CreateOrEditProductCustomerQueryModalComponent;
    @ViewChild('viewProductCustomerQueryModal', { static: true })
    viewProductCustomerQueryModal: ViewProductCustomerQueryModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    questionFilter = '';
    answerFilter = '';
    maxAnswerDateFilter: DateTime;
    minAnswerDateFilter: DateTime;
    answerTimeFilter = '';
    publishFilter = -1;
    productNameFilter = '';
    contactFullNameFilter = '';
    employeeNameFilter = '';

    constructor(
        injector: Injector,
        private _productCustomerQueriesServiceProxy: ProductCustomerQueriesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getProductCustomerQueries(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productCustomerQueriesServiceProxy
            .getAll(
                this.filterText,
                this.questionFilter,
                this.answerFilter,
                this.maxAnswerDateFilter === undefined
                    ? this.maxAnswerDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxAnswerDateFilter),
                this.minAnswerDateFilter === undefined
                    ? this.minAnswerDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minAnswerDateFilter),
                this.answerTimeFilter,
                this.publishFilter,
                this.productNameFilter,
                this.contactFullNameFilter,
                this.employeeNameFilter,
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

    createProductCustomerQuery(): void {
        this.createOrEditProductCustomerQueryModal.show();
    }

    deleteProductCustomerQuery(productCustomerQuery: ProductCustomerQueryDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._productCustomerQueriesServiceProxy.delete(productCustomerQuery.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._productCustomerQueriesServiceProxy
            .getProductCustomerQueriesToExcel(
                this.filterText,
                this.questionFilter,
                this.answerFilter,
                this.maxAnswerDateFilter === undefined
                    ? this.maxAnswerDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxAnswerDateFilter),
                this.minAnswerDateFilter === undefined
                    ? this.minAnswerDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minAnswerDateFilter),
                this.answerTimeFilter,
                this.publishFilter,
                this.productNameFilter,
                this.contactFullNameFilter,
                this.employeeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.questionFilter = '';
        this.answerFilter = '';
        this.maxAnswerDateFilter = undefined;
        this.minAnswerDateFilter = undefined;
        this.answerTimeFilter = '';
        this.publishFilter = -1;
        this.productNameFilter = '';
        this.contactFullNameFilter = '';
        this.employeeNameFilter = '';

        this.getProductCustomerQueries();
    }
}
