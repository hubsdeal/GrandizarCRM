import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreReviewsServiceProxy, CreateOrEditStoreReviewDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreReviewStoreLookupTableModalComponent } from './storeReview-store-lookup-table-modal.component';
import { StoreReviewContactLookupTableModalComponent } from './storeReview-contact-lookup-table-modal.component';
import { StoreReviewRatingLikeLookupTableModalComponent } from './storeReview-ratingLike-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreReviewModal',
    templateUrl: './create-or-edit-storeReview-modal.component.html',
})
export class CreateOrEditStoreReviewModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeReviewStoreLookupTableModal', { static: true })
    storeReviewStoreLookupTableModal: StoreReviewStoreLookupTableModalComponent;
    @ViewChild('storeReviewContactLookupTableModal', { static: true })
    storeReviewContactLookupTableModal: StoreReviewContactLookupTableModalComponent;
    @ViewChild('storeReviewRatingLikeLookupTableModal', { static: true })
    storeReviewRatingLikeLookupTableModal: StoreReviewRatingLikeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeReview: CreateOrEditStoreReviewDto = new CreateOrEditStoreReviewDto();

    storeName = '';
    contactFullName = '';
    ratingLikeName = '';

    constructor(
        injector: Injector,
        private _storeReviewsServiceProxy: StoreReviewsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeReviewId?: number): void {
        if (!storeReviewId) {
            this.storeReview = new CreateOrEditStoreReviewDto();
            this.storeReview.id = storeReviewId;
            this.storeReview.postDate = this._dateTimeService.getStartOfDay();
            this.storeName = '';
            this.contactFullName = '';
            this.ratingLikeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeReviewsServiceProxy.getStoreReviewForEdit(storeReviewId).subscribe((result) => {
                this.storeReview = result.storeReview;

                this.storeName = result.storeName;
                this.contactFullName = result.contactFullName;
                this.ratingLikeName = result.ratingLikeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._storeReviewsServiceProxy
            .createOrEdit(this.storeReview)
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

    openSelectStoreModal() {
        this.storeReviewStoreLookupTableModal.id = this.storeReview.storeId;
        this.storeReviewStoreLookupTableModal.displayName = this.storeName;
        this.storeReviewStoreLookupTableModal.show();
    }
    openSelectContactModal() {
        this.storeReviewContactLookupTableModal.id = this.storeReview.contactId;
        this.storeReviewContactLookupTableModal.displayName = this.contactFullName;
        this.storeReviewContactLookupTableModal.show();
    }
    openSelectRatingLikeModal() {
        this.storeReviewRatingLikeLookupTableModal.id = this.storeReview.ratingLikeId;
        this.storeReviewRatingLikeLookupTableModal.displayName = this.ratingLikeName;
        this.storeReviewRatingLikeLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeReview.storeId = null;
        this.storeName = '';
    }
    setContactIdNull() {
        this.storeReview.contactId = null;
        this.contactFullName = '';
    }
    setRatingLikeIdNull() {
        this.storeReview.ratingLikeId = null;
        this.ratingLikeName = '';
    }

    getNewStoreId() {
        this.storeReview.storeId = this.storeReviewStoreLookupTableModal.id;
        this.storeName = this.storeReviewStoreLookupTableModal.displayName;
    }
    getNewContactId() {
        this.storeReview.contactId = this.storeReviewContactLookupTableModal.id;
        this.contactFullName = this.storeReviewContactLookupTableModal.displayName;
    }
    getNewRatingLikeId() {
        this.storeReview.ratingLikeId = this.storeReviewRatingLikeLookupTableModal.id;
        this.ratingLikeName = this.storeReviewRatingLikeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
