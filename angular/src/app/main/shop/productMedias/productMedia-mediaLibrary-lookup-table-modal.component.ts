import { Component, ViewChild, Injector, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    ProductMediasServiceProxy,
    ProductMediaMediaLibraryLookupTableDto,
    MediaLibraryFromSpDto,
    MediaLibrariesServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
@Component({
    selector: 'productMediaMediaLibraryLookupTableModal',
    styleUrls: ['./productMedia-mediaLibrary-lookup-table-modal.component.less'],
    encapsulation: ViewEncapsulation.None,
    templateUrl: './productMedia-mediaLibrary-lookup-table-modal.component.html',
})
export class ProductMediaMediaLibraryLookupTableModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    filterText = '';
    id: number;
    displayName: string;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;
    employeeUserId:number;


    picture: string;

    selectedAll: boolean = false;
    selectedInput:any[] = [];
    @Output() selectedMedia: EventEmitter<MediaLibraryFromSpDto[]> = new EventEmitter<MediaLibraryFromSpDto[]>();
    skipCount: number=0;
    maxResultCount: number = 48;
    constructor(injector: Injector, private _productMediasServiceProxy: ProductMediasServiceProxy,
        private _mediaLibrariesServiceProxy: MediaLibrariesServiceProxy) {
        super(injector);
    }

   
    show(): void {
        this.active = true;
        //this.paginator.rows = 5;
        this.getAll();
        this.modal.show();
    }

    getAll(event?: LazyLoadEvent) {
        if (!this.active) {
            return;
        }

        // if (this.primengTableHelper.shouldResetPaging(event)) {
        //     this.paginator.changePage(0);
        //     if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
        //         return;
        //     }
        // }

        this.primengTableHelper.showLoadingIndicator();

        this._mediaLibrariesServiceProxy.getAllMediaLibrariesBySp(
            1,
            this.employeeUserId,
            this.filterText,
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            this.skipCount,
            this.maxResultCount
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.mediaLibraries;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    etAndSave(record:any) {
        this.id = record.id;
        this.displayName = record.name;
        this.picture=record.picture;
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }

    close(): void {
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }
    paginate(event: any) {
        this.skipCount = event.first;
        this.maxResultCount = event.rows;
        this.getAll();
    }

    checkIfAllSelected() {
        this.selectedAll = this.primengTableHelper.records.every(function (item: any) {
            return item.selected == true;
        })
    }

    refreshCheckboxReloadList() {
        this.selectedAll = false;
        for (var i = 0; i < this.primengTableHelper.records.length; i++) {
            this.primengTableHelper.records[i].selected = false;
        }
        this.reloadPage();
    }

    onSave(){
        this.selectedInput = this.primengTableHelper.records.filter(x => x.selected == true);
        this.selectedMedia.emit(this.selectedInput);
        this.modal.hide();
    }
}
