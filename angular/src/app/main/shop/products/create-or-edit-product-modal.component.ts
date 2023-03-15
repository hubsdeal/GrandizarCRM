import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProductsServiceProxy, CreateOrEditProductDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductProductCategoryLookupTableModalComponent } from './product-productCategory-lookup-table-modal.component';
import { ProductMediaLibraryLookupTableModalComponent } from './product-mediaLibrary-lookup-table-modal.component';
import { ProductMeasurementUnitLookupTableModalComponent } from './product-measurementUnit-lookup-table-modal.component';
import { ProductCurrencyLookupTableModalComponent } from './product-currency-lookup-table-modal.component';
import { ProductRatingLikeLookupTableModalComponent } from './product-ratingLike-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductModal',
    templateUrl: './create-or-edit-product-modal.component.html',
})
export class CreateOrEditProductModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productProductCategoryLookupTableModal', { static: true })
    productProductCategoryLookupTableModal: ProductProductCategoryLookupTableModalComponent;
    @ViewChild('productMediaLibraryLookupTableModal', { static: true })
    productMediaLibraryLookupTableModal: ProductMediaLibraryLookupTableModalComponent;
    @ViewChild('productMeasurementUnitLookupTableModal', { static: true })
    productMeasurementUnitLookupTableModal: ProductMeasurementUnitLookupTableModalComponent;
    @ViewChild('productCurrencyLookupTableModal', { static: true })
    productCurrencyLookupTableModal: ProductCurrencyLookupTableModalComponent;
    @ViewChild('productRatingLikeLookupTableModal', { static: true })
    productRatingLikeLookupTableModal: ProductRatingLikeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    product: CreateOrEditProductDto = new CreateOrEditProductDto();

    productCategoryName = '';
    mediaLibraryName = '';
    measurementUnitName = '';
    currencyName = '';
    ratingLikeName = '';

    constructor(
        injector: Injector,
        private _productsServiceProxy: ProductsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productId?: number): void {
        if (!productId) {
            this.product = new CreateOrEditProductDto();
            this.product.id = productId;
            this.productCategoryName = '';
            this.mediaLibraryName = '';
            this.measurementUnitName = '';
            this.currencyName = '';
            this.ratingLikeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productsServiceProxy.getProductForEdit(productId).subscribe((result) => {
                this.product = result.product;

                this.productCategoryName = result.productCategoryName;
                this.mediaLibraryName = result.mediaLibraryName;
                this.measurementUnitName = result.measurementUnitName;
                this.currencyName = result.currencyName;
                this.ratingLikeName = result.ratingLikeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._productsServiceProxy
            .createOrEdit(this.product)
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

    openSelectProductCategoryModal() {
        this.productProductCategoryLookupTableModal.id = this.product.productCategoryId;
        this.productProductCategoryLookupTableModal.displayName = this.productCategoryName;
        this.productProductCategoryLookupTableModal.show();
    }
    openSelectMediaLibraryModal() {
        this.productMediaLibraryLookupTableModal.id = this.product.mediaLibraryId;
        this.productMediaLibraryLookupTableModal.displayName = this.mediaLibraryName;
        this.productMediaLibraryLookupTableModal.show();
    }
    openSelectMeasurementUnitModal() {
        this.productMeasurementUnitLookupTableModal.id = this.product.measurementUnitId;
        this.productMeasurementUnitLookupTableModal.displayName = this.measurementUnitName;
        this.productMeasurementUnitLookupTableModal.show();
    }
    openSelectCurrencyModal() {
        this.productCurrencyLookupTableModal.id = this.product.currencyId;
        this.productCurrencyLookupTableModal.displayName = this.currencyName;
        this.productCurrencyLookupTableModal.show();
    }
    openSelectRatingLikeModal() {
        this.productRatingLikeLookupTableModal.id = this.product.ratingLikeId;
        this.productRatingLikeLookupTableModal.displayName = this.ratingLikeName;
        this.productRatingLikeLookupTableModal.show();
    }

    setProductCategoryIdNull() {
        this.product.productCategoryId = null;
        this.productCategoryName = '';
    }
    setMediaLibraryIdNull() {
        this.product.mediaLibraryId = null;
        this.mediaLibraryName = '';
    }
    setMeasurementUnitIdNull() {
        this.product.measurementUnitId = null;
        this.measurementUnitName = '';
    }
    setCurrencyIdNull() {
        this.product.currencyId = null;
        this.currencyName = '';
    }
    setRatingLikeIdNull() {
        this.product.ratingLikeId = null;
        this.ratingLikeName = '';
    }

    getNewProductCategoryId() {
        this.product.productCategoryId = this.productProductCategoryLookupTableModal.id;
        this.productCategoryName = this.productProductCategoryLookupTableModal.displayName;
    }
    getNewMediaLibraryId() {
        this.product.mediaLibraryId = this.productMediaLibraryLookupTableModal.id;
        this.mediaLibraryName = this.productMediaLibraryLookupTableModal.displayName;
    }
    getNewMeasurementUnitId() {
        this.product.measurementUnitId = this.productMeasurementUnitLookupTableModal.id;
        this.measurementUnitName = this.productMeasurementUnitLookupTableModal.displayName;
    }
    getNewCurrencyId() {
        this.product.currencyId = this.productCurrencyLookupTableModal.id;
        this.currencyName = this.productCurrencyLookupTableModal.displayName;
    }
    getNewRatingLikeId() {
        this.product.ratingLikeId = this.productRatingLikeLookupTableModal.id;
        this.ratingLikeName = this.productRatingLikeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
