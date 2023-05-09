import { Component, ViewChild, Injector, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ProductsServiceProxy, ProductMediaLibraryLookupTableDto, MediaLibrariesServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { DomSanitizer } from '@angular/platform-browser';
@Component({
    selector: 'productMediaLibraryLookupTableModal',
    styleUrls: ['./product-mediaLibrary-lookup-table-modal.component.less'],
    encapsulation: ViewEncapsulation.None,
    templateUrl: './product-mediaLibrary-lookup-table-modal.component.html',
})
export class ProductMediaLibraryLookupTableModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    filterText = '';
    id: number;
    displayName: string;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;

    constructor(injector: Injector, private _productsServiceProxy: ProductsServiceProxy, private _mediaLibraryServiceProxy: MediaLibrariesServiceProxy,
        private _sanitizer: DomSanitizer) {
        super(injector);
    }

    show(): void {
        this.active = true;
        this.paginator.rows = 5;
        this.getAll();
        this.modal.show();
    }

    getAll(event?: LazyLoadEvent) {
        if (!this.active) {
            return;
        }

        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        // this._productsServiceProxy
        //     .getAllMediaLibraryForLookupTable(
        //         this.filterText,
        //         this.primengTableHelper.getSorting(this.dataTable),
        //         this.primengTableHelper.getSkipCount(this.paginator, event),
        //         this.primengTableHelper.getMaxResultCount(this.paginator, event)
        //     )
        //     .subscribe((result) => {
        //         this.primengTableHelper.totalRecordsCount = result.totalCount;
        //         this.primengTableHelper.records = result.items;
        //         this.primengTableHelper.hideLoadingIndicator();
        //     });
        this._mediaLibraryServiceProxy.getAll(
            this.filterText,
            undefined,
            undefined,
            undefined,
            undefined,
            undefined,
            undefined,
            undefined,
            undefined,
            undefined,
            undefined,
            undefined,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        })
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    setAndSave(id:number, name:string) {
        this.id = id;
        this.displayName = name;
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }

    close(): void {
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }
    getSafeEmbeddedVideoUrl(url: string) {
        return this._sanitizer.bypassSecurityTrustResourceUrl(url);
    }
}
