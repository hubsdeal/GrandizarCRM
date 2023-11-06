import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductNotesServiceProxy, ProductNoteDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductNoteModalComponent } from './create-or-edit-productNote-modal.component';

import { ViewProductNoteModalComponent } from './view-productNote-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'app-productNotes',
    templateUrl: './productNotes.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductNotesComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditProductNoteModal', { static: true })
    createOrEditProductNoteModal: CreateOrEditProductNoteModalComponent;
    @ViewChild('viewProductNoteModal', { static: true }) viewProductNoteModal: ViewProductNoteModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    notesFilter = '';
    productNameFilter = '';

    @Input() productId: number;
    allNotes: any[] = [];

    constructor(
        injector: Injector,
        private _productNotesServiceProxy: ProductNotesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        
    }

    ngOnInit(): void {
        this.getAllProductNotes(this.productId);
    }

    getAllProductNotes(id:number) {
        this._productNotesServiceProxy
            .getAll(
                this.filterText,
                this.notesFilter,
                this.productNameFilter,
                id,
                '',
                0,
                10000
            )
            .subscribe((result) => {
                this.allNotes = result.items;
            });
    }

    getProductNotes(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        // this._productNotesServiceProxy
        //     .getAll(
        //         this.filterText,
        //         this.notesFilter,
        //         this.productNameFilter,
        //         this.productId,
        //         this.primengTableHelper.getSorting(this.dataTable),
        //         this.primengTableHelper.getSkipCount(this.paginator, event),
        //         this.primengTableHelper.getMaxResultCount(this.paginator, event)
        //     )
        //     .subscribe((result) => {
        //         this.primengTableHelper.totalRecordsCount = result.totalCount;
        //         this.primengTableHelper.records = result.items;
        //         this.primengTableHelper.hideLoadingIndicator();
        //     });
    }

    reloadPage(): void {
        this.getAllProductNotes(this.productId);
    }

    createProductNote(): void {
        this.createOrEditProductNoteModal.productId = this.productId;
        this.createOrEditProductNoteModal.show();
    }

    deleteProductNote(productNote: ProductNoteDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._productNotesServiceProxy.delete(productNote.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._productNotesServiceProxy
            .getProductNotesToExcel(this.filterText, this.notesFilter, this.productNameFilter)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.notesFilter = '';
        this.productNameFilter = '';

        this.getProductNotes();
    }
}
