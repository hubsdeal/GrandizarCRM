import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetRewardPointTypeForViewDto, RewardPointTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewRewardPointTypeModal',
    templateUrl: './view-rewardPointType-modal.component.html'
})
export class ViewRewardPointTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetRewardPointTypeForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetRewardPointTypeForViewDto();
        this.item.rewardPointType = new RewardPointTypeDto();
    }

    show(item: GetRewardPointTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
