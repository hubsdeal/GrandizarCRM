import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetRatingLikeForViewDto, RatingLikeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewRatingLikeModal',
    templateUrl: './view-ratingLike-modal.component.html',
})
export class ViewRatingLikeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetRatingLikeForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetRatingLikeForViewDto();
        this.item.ratingLike = new RatingLikeDto();
    }

    show(item: GetRatingLikeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
