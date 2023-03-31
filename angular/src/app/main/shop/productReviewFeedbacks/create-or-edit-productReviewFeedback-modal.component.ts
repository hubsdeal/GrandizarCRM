import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductReviewFeedbacksServiceProxy,
    CreateOrEditProductReviewFeedbackDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductReviewFeedbackContactLookupTableModalComponent } from './productReviewFeedback-contact-lookup-table-modal.component';
import { ProductReviewFeedbackProductReviewLookupTableModalComponent } from './productReviewFeedback-productReview-lookup-table-modal.component';
import { ProductReviewFeedbackRatingLikeLookupTableModalComponent } from './productReviewFeedback-ratingLike-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductReviewFeedbackModal',
    templateUrl: './create-or-edit-productReviewFeedback-modal.component.html',
})
export class CreateOrEditProductReviewFeedbackModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productReviewFeedbackContactLookupTableModal', { static: true })
    productReviewFeedbackContactLookupTableModal: ProductReviewFeedbackContactLookupTableModalComponent;
    @ViewChild('productReviewFeedbackProductReviewLookupTableModal', { static: true })
    productReviewFeedbackProductReviewLookupTableModal: ProductReviewFeedbackProductReviewLookupTableModalComponent;
    @ViewChild('productReviewFeedbackRatingLikeLookupTableModal', { static: true })
    productReviewFeedbackRatingLikeLookupTableModal: ProductReviewFeedbackRatingLikeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productReviewFeedback: CreateOrEditProductReviewFeedbackDto = new CreateOrEditProductReviewFeedbackDto();

    contactFullName = '';
    productReviewReviewInfo = '';
    ratingLikeName = '';

    constructor(
        injector: Injector,
        private _productReviewFeedbacksServiceProxy: ProductReviewFeedbacksServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productReviewFeedbackId?: number): void {
        if (!productReviewFeedbackId) {
            this.productReviewFeedback = new CreateOrEditProductReviewFeedbackDto();
            this.productReviewFeedback.id = productReviewFeedbackId;
            this.contactFullName = '';
            this.productReviewReviewInfo = '';
            this.ratingLikeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productReviewFeedbacksServiceProxy
                .getProductReviewFeedbackForEdit(productReviewFeedbackId)
                .subscribe((result) => {
                    this.productReviewFeedback = result.productReviewFeedback;

                    this.contactFullName = result.contactFullName;
                    this.productReviewReviewInfo = result.productReviewReviewInfo;
                    this.ratingLikeName = result.ratingLikeName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._productReviewFeedbacksServiceProxy
            .createOrEdit(this.productReviewFeedback)
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
        this.productReviewFeedbackContactLookupTableModal.id = this.productReviewFeedback.contactId;
        this.productReviewFeedbackContactLookupTableModal.displayName = this.contactFullName;
        this.productReviewFeedbackContactLookupTableModal.show();
    }
    openSelectProductReviewModal() {
        this.productReviewFeedbackProductReviewLookupTableModal.id = this.productReviewFeedback.productReviewId;
        this.productReviewFeedbackProductReviewLookupTableModal.displayName = this.productReviewReviewInfo;
        this.productReviewFeedbackProductReviewLookupTableModal.show();
    }
    openSelectRatingLikeModal() {
        this.productReviewFeedbackRatingLikeLookupTableModal.id = this.productReviewFeedback.ratingLikeId;
        this.productReviewFeedbackRatingLikeLookupTableModal.displayName = this.ratingLikeName;
        this.productReviewFeedbackRatingLikeLookupTableModal.show();
    }

    setContactIdNull() {
        this.productReviewFeedback.contactId = null;
        this.contactFullName = '';
    }
    setProductReviewIdNull() {
        this.productReviewFeedback.productReviewId = null;
        this.productReviewReviewInfo = '';
    }
    setRatingLikeIdNull() {
        this.productReviewFeedback.ratingLikeId = null;
        this.ratingLikeName = '';
    }

    getNewContactId() {
        this.productReviewFeedback.contactId = this.productReviewFeedbackContactLookupTableModal.id;
        this.contactFullName = this.productReviewFeedbackContactLookupTableModal.displayName;
    }
    getNewProductReviewId() {
        this.productReviewFeedback.productReviewId = this.productReviewFeedbackProductReviewLookupTableModal.id;
        this.productReviewReviewInfo = this.productReviewFeedbackProductReviewLookupTableModal.displayName;
    }
    getNewRatingLikeId() {
        this.productReviewFeedback.ratingLikeId = this.productReviewFeedbackRatingLikeLookupTableModal.id;
        this.ratingLikeName = this.productReviewFeedbackRatingLikeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
