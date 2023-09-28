import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetContactProductRecommendationForViewDto, ContactProductRecommendationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewContactProductRecommendationModal',
    templateUrl: './view-contactProductRecommendation-modal.component.html'
})
export class ViewContactProductRecommendationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetContactProductRecommendationForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetContactProductRecommendationForViewDto();
        this.item.contactProductRecommendation = new ContactProductRecommendationDto();
    }

    show(item: GetContactProductRecommendationForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
