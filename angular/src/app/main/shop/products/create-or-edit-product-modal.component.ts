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
import { ProductContactLookupTableModalComponent } from './product-contact-lookup-table-modal.component';
import { ProductStoreLookupTableModalComponent } from './product-store-lookup-table-modal.component';
import { Router } from '@angular/router';
import { result } from 'lodash-es';
import { SelectItem } from 'primeng/api';
import { MatDialog } from '@angular/material/dialog';
import { Title } from '@angular/platform-browser';
import { ChatGptResponseModalComponent } from '@app/shared/chat-gpt-response-modal/chat-gpt-response-modal.component';

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
    @ViewChild('productContactLookupTableModal', { static: true })
    productContactLookupTableModal: ProductContactLookupTableModalComponent;
    @ViewChild('productStoreLookupTableModal', { static: true })
    productStoreLookupTableModal: ProductStoreLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    product: CreateOrEditProductDto = new CreateOrEditProductDto();

    productCategoryName = '';
    mediaLibraryName = '';
    measurementUnitName = '';
    currencyName = '';
    ratingLikeName = '';
    contactFullName = '';
    storeName = '';

    productCategoryOptions: any = []
    selectedProductCategory: any;

    productLibraryOptions: any = []
    selectedProductLibrary: any;


    productServiceOptions: SelectItem[];

    isTemplate: boolean = false;
    constructor(
        injector: Injector,
        private _productsServiceProxy: ProductsServiceProxy,
        private _dateTimeService: DateTimeService,
        private _router: Router,
        private dialog: MatDialog,
        private titleService: Title
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
            this.contactFullName = '';
            this.storeName = '';
            this.product.isService = true;
            if (this.isTemplate) {
                this.product.isTemplate = true;
            } else {
                this.product.isTemplate = false;
            }

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
                this.contactFullName = result.contactFullName;
                this.storeName = result.storeName;

                this.active = true;
                this.modal.show();
            });
        }

        this._productsServiceProxy.getAllProductCategoryForLookupTable('', '', 0, 10000).subscribe(result => {
            this.productCategoryOptions = result.items;
        });
        this._productsServiceProxy.getAllProductLibraryForTableDropdown().subscribe(result => {
            this.productLibraryOptions = result;
        });
        this.productServiceOptions = [{ label: 'Product', value: true }, { label: 'Service', value: false }];
    }
    onProductCategoryClick(event: any) {
        if (event.value != null) {
            this.product.productCategoryId = event.value.id;
        }
    }

    onProductLibraryClick(event: any) {
        if (event.value != null) {
            this._productsServiceProxy.getProductForEdit(event.value.id).subscribe((result) => {
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
    save(): void {
        this.saving = true;

        if (this.isTemplate) {
            this.product.isTemplate = true;
        } else {
            this.product.isTemplate = false;
        }

        this._productsServiceProxy
            .createOrEdit(this.product)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(result => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
                this._router.navigate(['/app/main/shop/products/dashboard/', result])
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
    openSelectContactModal() {
        this.productContactLookupTableModal.id = this.product.contactId;
        this.productContactLookupTableModal.displayName = this.contactFullName;
        this.productContactLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.productStoreLookupTableModal.id = this.product.storeId;
        this.productStoreLookupTableModal.displayName = this.storeName;
        this.productStoreLookupTableModal.show();
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
    setContactIdNull() {
        this.product.contactId = null;
        this.contactFullName = '';
    }
    setStoreIdNull() {
        this.product.storeId = null;
        this.storeName = '';
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
    getNewContactId() {
        this.product.contactId = this.productContactLookupTableModal.id;
        this.contactFullName = this.productContactLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.product.storeId = this.productStoreLookupTableModal.id;
        this.storeName = this.productStoreLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void { }

    openAiModalProductCreate(): void {
        const productShortDesc = "Write a  short description for a product where product name is Organic Mustard Oil and product brand is Saffola"
        var modalTitle = "AI Text Generator - Product Description"
        const dialogRef = this.dialog.open(ChatGptResponseModalComponent, {
          data: { promtFromAnotherComponent: productShortDesc, feildName: '', modalTitle: modalTitle },
          width: '1100px',
        });
    
        dialogRef.afterClosed().subscribe(result => {
          console.log(result)
          //this.bindingData = result.data;
        });
      }
}
