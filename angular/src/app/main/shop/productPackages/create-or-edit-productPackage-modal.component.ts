import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProductPackagesServiceProxy, CreateOrEditProductPackageDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductPackageProductLookupTableModalComponent } from './productPackage-product-lookup-table-modal.component';
import { ProductPackageMediaLibraryLookupTableModalComponent } from './productPackage-mediaLibrary-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductPackageModal',
    templateUrl: './create-or-edit-productPackage-modal.component.html',
})
export class CreateOrEditProductPackageModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productPackageProductLookupTableModal', { static: true })
    productPackageProductLookupTableModal: ProductPackageProductLookupTableModalComponent;
    @ViewChild('productPackageMediaLibraryLookupTableModal', { static: true })
    productPackageMediaLibraryLookupTableModal: ProductPackageMediaLibraryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productPackage: CreateOrEditProductPackageDto = new CreateOrEditProductPackageDto();

    productName = '';
    mediaLibraryName = '';

    constructor(
        injector: Injector,
        private _productPackagesServiceProxy: ProductPackagesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productPackageId?: number): void {
        if (!productPackageId) {
            this.productPackage = new CreateOrEditProductPackageDto();
            this.productPackage.id = productPackageId;
            this.productName = '';
            this.mediaLibraryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productPackagesServiceProxy.getProductPackageForEdit(productPackageId).subscribe((result) => {
                this.productPackage = result.productPackage;

                this.productName = result.productName;
                this.mediaLibraryName = result.mediaLibraryName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._productPackagesServiceProxy
            .createOrEdit(this.productPackage)
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
        this.productPackageProductLookupTableModal.id = this.productPackage.primaryProductId;
        this.productPackageProductLookupTableModal.displayName = this.productName;
        this.productPackageProductLookupTableModal.show();
    }
    openSelectMediaLibraryModal() {
        this.productPackageMediaLibraryLookupTableModal.id = this.productPackage.mediaLibraryId;
        this.productPackageMediaLibraryLookupTableModal.displayName = this.mediaLibraryName;
        this.productPackageMediaLibraryLookupTableModal.show();
    }

    setPrimaryProductIdNull() {
        this.productPackage.primaryProductId = null;
        this.productName = '';
    }
    setMediaLibraryIdNull() {
        this.productPackage.mediaLibraryId = null;
        this.mediaLibraryName = '';
    }

    getNewPrimaryProductId() {
        this.productPackage.primaryProductId = this.productPackageProductLookupTableModal.id;
        this.productName = this.productPackageProductLookupTableModal.displayName;
    }
    getNewMediaLibraryId() {
        this.productPackage.mediaLibraryId = this.productPackageMediaLibraryLookupTableModal.id;
        this.mediaLibraryName = this.productPackageMediaLibraryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
