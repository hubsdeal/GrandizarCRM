import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreReviewForViewDto, StoreReviewDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreReviewModal',
    templateUrl: './view-storeReview-modal.component.html',
})
export class ViewStoreReviewModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreReviewForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreReviewForViewDto();
        this.item.storeReview = new StoreReviewDto();
    }

    show(item: GetStoreReviewForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
