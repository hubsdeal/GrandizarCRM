import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProductReviewsServiceProxy, CreateOrEditProductReviewDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductReviewContactLookupTableModalComponent } from './productReview-contact-lookup-table-modal.component';
import { ProductReviewProductLookupTableModalComponent } from './productReview-product-lookup-table-modal.component';
import { ProductReviewStoreLookupTableModalComponent } from './productReview-store-lookup-table-modal.component';
import { ProductReviewRatingLikeLookupTableModalComponent } from './productReview-ratingLike-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductReviewModal',
    templateUrl: './create-or-edit-productReview-modal.component.html',
})
export class CreateOrEditProductReviewModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productReviewContactLookupTableModal', { static: true })
    productReviewContactLookupTableModal: ProductReviewContactLookupTableModalComponent;
    @ViewChild('productReviewProductLookupTableModal', { static: true })
    productReviewProductLookupTableModal: ProductReviewProductLookupTableModalComponent;
    @ViewChild('productReviewStoreLookupTableModal', { static: true })
    productReviewStoreLookupTableModal: ProductReviewStoreLookupTableModalComponent;
    @ViewChild('productReviewRatingLikeLookupTableModal', { static: true })
    productReviewRatingLikeLookupTableModal: ProductReviewRatingLikeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productReview: CreateOrEditProductReviewDto = new CreateOrEditProductReviewDto();

    contactFullName = '';
    productName = '';
    storeName = '';
    ratingLikeName = '';

    constructor(
        injector: Injector,
        private _productReviewsServiceProxy: ProductReviewsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productReviewId?: number): void {
        if (!productReviewId) {
            this.productReview = new CreateOrEditProductReviewDto();
            this.productReview.id = productReviewId;
            this.productReview.postDate = this._dateTimeService.getStartOfDay();
            this.contactFullName = '';
            this.productName = '';
            this.storeName = '';
            this.ratingLikeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productReviewsServiceProxy.getProductReviewForEdit(productReviewId).subscribe((result) => {
                this.productReview = result.productReview;

                this.contactFullName = result.contactFullName;
                this.productName = result.productName;
                this.storeName = result.storeName;
                this.ratingLikeName = result.ratingLikeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._productReviewsServiceProxy
            .createOrEdit(this.productReview)
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

    openSelectContactModal() {
        this.productReviewContactLookupTableModal.id = this.productReview.contactId;
        this.productReviewContactLookupTableModal.displayName = this.contactFullName;
        this.productReviewContactLookupTableModal.show();
    }
    openSelectProductModal() {
        this.productReviewProductLookupTableModal.id = this.productReview.productId;
        this.productReviewProductLookupTableModal.displayName = this.productName;
        this.productReviewProductLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.productReviewStoreLookupTableModal.id = this.productReview.storeId;
        this.productReviewStoreLookupTableModal.displayName = this.storeName;
        this.productReviewStoreLookupTableModal.show();
    }
    openSelectRatingLikeModal() {
        this.productReviewRatingLikeLookupTableModal.id = this.productReview.ratingLikeId;
        this.productReviewRatingLikeLookupTableModal.displayName = this.ratingLikeName;
        this.productReviewRatingLikeLookupTableModal.show();
    }

    setContactIdNull() {
        this.productReview.contactId = null;
        this.contactFullName = '';
    }
    setProductIdNull() {
        this.productReview.productId = null;
        this.productName = '';
    }
    setStoreIdNull() {
        this.productReview.storeId = null;
        this.storeName = '';
    }
    setRatingLikeIdNull() {
        this.productReview.ratingLikeId = null;
        this.ratingLikeName = '';
    }

    getNewContactId() {
        this.productReview.contactId = this.productReviewContactLookupTableModal.id;
        this.contactFullName = this.productReviewContactLookupTableModal.displayName;
    }
    getNewProductId() {
        this.productReview.productId = this.productReviewProductLookupTableModal.id;
        this.productName = this.productReviewProductLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.productReview.storeId = this.productReviewStoreLookupTableModal.id;
        this.storeName = this.productReviewStoreLookupTableModal.displayName;
    }
    getNewRatingLikeId() {
        this.productReview.ratingLikeId = this.productReviewRatingLikeLookupTableModal.id;
        this.ratingLikeName = this.productReviewRatingLikeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
