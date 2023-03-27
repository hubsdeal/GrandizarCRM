import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreReviewFeedbackForViewDto, StoreReviewFeedbackDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreReviewFeedbackModal',
    templateUrl: './view-storeReviewFeedback-modal.component.html',
})
export class ViewStoreReviewFeedbackModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreReviewFeedbackForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreReviewFeedbackForViewDto();
        this.item.storeReviewFeedback = new StoreReviewFeedbackDto();
    }

    show(item: GetStoreReviewFeedbackForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
