import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreProductMapsServiceProxy, CreateOrEditStoreProductMapDto, ProductsServiceProxy, StoresServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreProductMapStoreLookupTableModalComponent } from './storeProductMap-store-lookup-table-modal.component';
import { StoreProductMapProductLookupTableModalComponent } from './storeProductMap-product-lookup-table-modal.component';
import { CreateOrEditProductDto } from '@shared/service-proxies/service-proxies';
import { SelectItem } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
    selector: 'createOrEditStoreProductMapModal',
    templateUrl: './create-or-edit-storeProductMap-modal.component.html',
})
export class CreateOrEditStoreProductMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeProductMapStoreLookupTableModal', { static: true })
    storeProductMapStoreLookupTableModal: StoreProductMapStoreLookupTableModalComponent;
    @ViewChild('storeProductMapProductLookupTableModal', { static: true })
    storeProductMapProductLookupTableModal: StoreProductMapProductLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeProductMap: CreateOrEditStoreProductMapDto = new CreateOrEditStoreProductMapDto();

    storeName = '';
    productName = '';

    storeId: number;
    product: CreateOrEditProductDto = new CreateOrEditProductDto();
    productServiceOptions: SelectItem[];
    productCategoryOptions: any = []
    selectedProductCategory: any;

    productId: number;
    selectedInput: number[] = [];
    bulkProductAssignToStore: boolean = false;
    bulkProductTemplateAssignToStore: boolean = false;

    constructor(
        injector: Injector,
        private _storeProductMapsServiceProxy: StoreProductMapsServiceProxy,
        private _productsServiceProxy: ProductsServiceProxy,
        private _storesServiceProxy: StoresServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeProductMapId?: number): void {
        if (!storeProductMapId) {
            this.storeProductMap = new CreateOrEditStoreProductMapDto();
            this.storeProductMap.id = storeProductMapId;
            this.storeName = '';
            this.productName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeProductMapsServiceProxy.getStoreProductMapForEdit(storeProductMapId).subscribe((result) => {
                this.storeProductMap = result.storeProductMap;
                this.storeId = result.storeProductMap.storeId;
                this.storeName = result.storeName;
                this.productName = result.productName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    // save(): void {
    //     this.saving = true;
    //     this.storeProductMap.storeId = this.storeId
    //     this._storeProductMapsServiceProxy
    //         .createOrEdit(this.storeProductMap)
    //         .pipe(
    //             finalize(() => {
    //                 this.saving = false;
    //             })
    //         )
    //         .subscribe(() => {
    //             this.notify.info(this.l('SavedSuccessfully'));
    //             this.close();
    //             this.modalSave.emit(null);
    //         });
    // }
    save(): void {
        this.saving = true;
        if (this.bulkProductTemplateAssignToStore) {
            this._storesServiceProxy.bulkProductAssignFromProductLibrary(this.storeProductMap.storeId, this.selectedInput)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(() => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modalSave.emit(null);
                });
        } else {
            if (this.storeId) {
                this.product.storeId = this.storeId;
            }
            this._productsServiceProxy.createOrEdit(this.product)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(result => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modalSave.emit(null);
                    this._router.navigate(['/app/main/shop/products/dashboard/', result]);
                });
        }
    }

    openSelectStoreModal() {
        this.storeProductMapStoreLookupTableModal.id = this.storeProductMap.storeId;
        this.storeProductMapStoreLookupTableModal.displayName = this.storeName;
        this.storeProductMapStoreLookupTableModal.show();
    }
    openSelectProductModal() {
        this.storeProductMapProductLookupTableModal.id = this.storeProductMap.productId;
        this.storeProductMapProductLookupTableModal.displayName = this.productName;
        this.storeProductMapProductLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeProductMap.storeId = null;
        this.storeName = '';
    }
    setProductIdNull() {
        this.storeProductMap.productId = null;
        this.productName = '';
    }

    getNewStoreId() {
        this.storeProductMap.storeId = this.storeProductMapStoreLookupTableModal.id;
        this.storeName = this.storeProductMapStoreLookupTableModal.displayName;
    }
    getNewProductId() {
        this.storeProductMap.productId = this.storeProductMapProductLookupTableModal.id;
        this.productName = this.storeProductMapProductLookupTableModal.displayName;
        if (this.storeProductMapProductLookupTableModal.id) {
            this._productsServiceProxy.getProductForEdit(this.storeProductMapProductLookupTableModal.id).subscribe((result) => {
                this.product.name = result.product.name;
                this.product.storeId = result.product.storeId;
                this.product.productCategoryId = result.product.productCategoryId;
                this.product.mediaLibraryId = result.product.mediaLibraryId;
                this.product.measurementUnitId = result.product.measurementUnitId;
                this.product.currencyId = result.product.currencyId;
                this.product.ratingLikeId = result.product.ratingLikeId;
                this.product.contactId = result.product.contactId;
                this.product.regularPrice = result.product.regularPrice;
                this.product.salePrice = result.product.salePrice;
                this.product.unitPrice = result.product.unitPrice;
                this.product.measurementAmount = result.product.measurementAmount;
                this.product.isService = result.product.isService;
                this.product.stockQuantity = result.product.stockQuantity;
                this.product.sku = result.product.sku;
                this.product.url = result.product.url;
                this.product.seoTitle = result.product.seoTitle;
                this.product.metaKeywords = result.product.metaKeywords;
                this.product.metaDescription = result.product.metaDescription;
                this.product.shortDescription = result.product.shortDescription;
                this.product.description = result.product.description;
                this.product.internalNotes = result.product.internalNotes;
                this.product.isPackageProduct = result.product.isPackageProduct;
                this.product.isWholeSaleProduct = result.product.isWholeSaleProduct;
                this.product.isPublished = result.product.isPublished;
                this.product.isTaxExempt = result.product.isTaxExempt;
                this.product.callForPrice = result.product.callForPrice;
            });
        }
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void { }
}
