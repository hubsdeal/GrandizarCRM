import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProductMediasServiceProxy, CreateOrEditProductMediaDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductMediaProductLookupTableModalComponent } from './productMedia-product-lookup-table-modal.component';
import { ProductMediaMediaLibraryLookupTableModalComponent } from './productMedia-mediaLibrary-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductMediaModal',
    templateUrl: './create-or-edit-productMedia-modal.component.html',
})
export class CreateOrEditProductMediaModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productMediaProductLookupTableModal', { static: true })
    productMediaProductLookupTableModal: ProductMediaProductLookupTableModalComponent;
    @ViewChild('productMediaMediaLibraryLookupTableModal', { static: true })
    productMediaMediaLibraryLookupTableModal: ProductMediaMediaLibraryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productMedia: CreateOrEditProductMediaDto = new CreateOrEditProductMediaDto();

    productName = '';
    mediaLibraryName = '';

    constructor(
        injector: Injector,
        private _productMediasServiceProxy: ProductMediasServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productMediaId?: number): void {
        if (!productMediaId) {
            this.productMedia = new CreateOrEditProductMediaDto();
            this.productMedia.id = productMediaId;
            this.productName = '';
            this.mediaLibraryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productMediasServiceProxy.getProductMediaForEdit(productMediaId).subscribe((result) => {
                this.productMedia = result.productMedia;

                this.productName = result.productName;
                this.mediaLibraryName = result.mediaLibraryName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._productMediasServiceProxy
            .createOrEdit(this.productMedia)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    openSelectProductModal() {
        this.productMediaProductLookupTableModal.id = this.productMedia.productId;
        this.productMediaProductLookupTableModal.displayName = this.productName;
        this.productMediaProductLookupTableModal.show();
    }
    openSelectMediaLibraryModal() {
        this.productMediaMediaLibraryLookupTableModal.id = this.productMedia.mediaLibraryId;
        this.productMediaMediaLibraryLookupTableModal.displayName = this.mediaLibraryName;
        this.productMediaMediaLibraryLookupTableModal.show();
    }

    setProductIdNull() {
        this.productMedia.productId = null;
        this.productName = '';
    }
    setMediaLibraryIdNull() {
        this.productMedia.mediaLibraryId = null;
        this.mediaLibraryName = '';
    }

    getNewProductId() {
        this.productMedia.productId = this.productMediaProductLookupTableModal.id;
        this.productName = this.productMediaProductLookupTableModal.displayName;
    }
    getNewMediaLibraryId() {
        this.productMedia.mediaLibraryId = this.productMediaMediaLibraryLookupTableModal.id;
        this.mediaLibraryName = this.productMediaMediaLibraryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
