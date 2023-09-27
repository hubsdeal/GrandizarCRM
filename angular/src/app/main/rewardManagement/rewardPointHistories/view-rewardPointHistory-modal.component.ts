import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetRewardPointHistoryForViewDto, RewardPointHistoryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewRewardPointHistoryModal',
    templateUrl: './view-rewardPointHistory-modal.component.html'
})
export class ViewRewardPointHistoryModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetRewardPointHistoryForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetRewardPointHistoryForViewDto();
        this.item.rewardPointHistory = new RewardPointHistoryDto();
    }

    show(item: GetRewardPointHistoryForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
