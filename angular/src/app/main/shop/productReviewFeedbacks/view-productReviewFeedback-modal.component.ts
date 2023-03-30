import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetProductReviewFeedbackForViewDto, ProductReviewFeedbackDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductReviewFeedbackModal',
    templateUrl: './view-productReviewFeedback-modal.component.html',
})
export class ViewProductReviewFeedbackModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductReviewFeedbackForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductReviewFeedbackForViewDto();
        this.item.productReviewFeedback = new ProductReviewFeedbackDto();
    }

    show(item: GetProductReviewFeedbackForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
