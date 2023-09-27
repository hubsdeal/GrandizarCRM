import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetRewardPointAwardSettingForViewDto, RewardPointAwardSettingDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewRewardPointAwardSettingModal',
    templateUrl: './view-rewardPointAwardSetting-modal.component.html'
})
export class ViewRewardPointAwardSettingModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetRewardPointAwardSettingForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetRewardPointAwardSettingForViewDto();
        this.item.rewardPointAwardSetting = new RewardPointAwardSettingDto();
    }

    show(item: GetRewardPointAwardSettingForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
