import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetRewardPointPriceSettingForViewDto, RewardPointPriceSettingDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewRewardPointPriceSettingModal',
    templateUrl: './view-rewardPointPriceSetting-modal.component.html'
})
export class ViewRewardPointPriceSettingModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetRewardPointPriceSettingForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetRewardPointPriceSettingForViewDto();
        this.item.rewardPointPriceSetting = new RewardPointPriceSettingDto();
    }

    show(item: GetRewardPointPriceSettingForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
