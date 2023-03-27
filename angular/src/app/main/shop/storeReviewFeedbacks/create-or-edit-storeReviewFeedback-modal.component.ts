import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    StoreReviewFeedbacksServiceProxy,
    CreateOrEditStoreReviewFeedbackDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreReviewFeedbackStoreReviewLookupTableModalComponent } from './storeReviewFeedback-storeReview-lookup-table-modal.component';
import { StoreReviewFeedbackContactLookupTableModalComponent } from './storeReviewFeedback-contact-lookup-table-modal.component';
import { StoreReviewFeedbackRatingLikeLookupTableModalComponent } from './storeReviewFeedback-ratingLike-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreReviewFeedbackModal',
    templateUrl: './create-or-edit-storeReviewFeedback-modal.component.html',
})
export class CreateOrEditStoreReviewFeedbackModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeReviewFeedbackStoreReviewLookupTableModal', { static: true })
    storeReviewFeedbackStoreReviewLookupTableModal: StoreReviewFeedbackStoreReviewLookupTableModalComponent;
    @ViewChild('storeReviewFeedbackContactLookupTableModal', { static: true })
    storeReviewFeedbackContactLookupTableModal: StoreReviewFeedbackContactLookupTableModalComponent;
    @ViewChild('storeReviewFeedbackRatingLikeLookupTableModal', { static: true })
    storeReviewFeedbackRatingLikeLookupTableModal: StoreReviewFeedbackRatingLikeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeReviewFeedback: CreateOrEditStoreReviewFeedbackDto = new CreateOrEditStoreReviewFeedbackDto();

    storeReviewReviewInfo = '';
    contactFullName = '';
    ratingLikeName = '';

    constructor(
        injector: Injector,
        private _storeReviewFeedbacksServiceProxy: StoreReviewFeedbacksServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeReviewFeedbackId?: number): void {
        if (!storeReviewFeedbackId) {
            this.storeReviewFeedback = new CreateOrEditStoreReviewFeedbackDto();
            this.storeReviewFeedback.id = storeReviewFeedbackId;
            this.storeReviewReviewInfo = '';
            this.contactFullName = '';
            this.ratingLikeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeReviewFeedbacksServiceProxy
                .getStoreReviewFeedbackForEdit(storeReviewFeedbackId)
                .subscribe((result) => {
                    this.storeReviewFeedback = result.storeReviewFeedback;

                    this.storeReviewReviewInfo = result.storeReviewReviewInfo;
                    this.contactFullName = result.contactFullName;
                    this.ratingLikeName = result.ratingLikeName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._storeReviewFeedbacksServiceProxy
            .createOrEdit(this.storeReviewFeedback)
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

    openSelectStoreReviewModal() {
        this.storeReviewFeedbackStoreReviewLookupTableModal.id = this.storeReviewFeedback.storeReviewId;
        this.storeReviewFeedbackStoreReviewLookupTableModal.displayName = this.storeReviewReviewInfo;
        this.storeReviewFeedbackStoreReviewLookupTableModal.show();
    }
    openSelectContactModal() {
        this.storeReviewFeedbackContactLookupTableModal.id = this.storeReviewFeedback.contactId;
        this.storeReviewFeedbackContactLookupTableModal.displayName = this.contactFullName;
        this.storeReviewFeedbackContactLookupTableModal.show();
    }
    openSelectRatingLikeModal() {
        this.storeReviewFeedbackRatingLikeLookupTableModal.id = this.storeReviewFeedback.ratingLikeId;
        this.storeReviewFeedbackRatingLikeLookupTableModal.displayName = this.ratingLikeName;
        this.storeReviewFeedbackRatingLikeLookupTableModal.show();
    }

    setStoreReviewIdNull() {
        this.storeReviewFeedback.storeReviewId = null;
        this.storeReviewReviewInfo = '';
    }
    setContactIdNull() {
        this.storeReviewFeedback.contactId = null;
        this.contactFullName = '';
    }
    setRatingLikeIdNull() {
        this.storeReviewFeedback.ratingLikeId = null;
        this.ratingLikeName = '';
    }

    getNewStoreReviewId() {
        this.storeReviewFeedback.storeReviewId = this.storeReviewFeedbackStoreReviewLookupTableModal.id;
        this.storeReviewReviewInfo = this.storeReviewFeedbackStoreReviewLookupTableModal.displayName;
    }
    getNewContactId() {
        this.storeReviewFeedback.contactId = this.storeReviewFeedbackContactLookupTableModal.id;
        this.contactFullName = this.storeReviewFeedbackContactLookupTableModal.displayName;
    }
    getNewRatingLikeId() {
        this.storeReviewFeedback.ratingLikeId = this.storeReviewFeedbackRatingLikeLookupTableModal.id;
        this.ratingLikeName = this.storeReviewFeedbackRatingLikeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
