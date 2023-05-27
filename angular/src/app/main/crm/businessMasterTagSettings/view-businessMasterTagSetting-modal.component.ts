import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    GetBusinessMasterTagSettingForViewDto,
    BusinessMasterTagSettingDto,
    AnswerType,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewBusinessMasterTagSettingModal',
    templateUrl: './view-businessMasterTagSetting-modal.component.html',
})
export class ViewBusinessMasterTagSettingModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetBusinessMasterTagSettingForViewDto;
    answerType = AnswerType;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetBusinessMasterTagSettingForViewDto();
        this.item.businessMasterTagSetting = new BusinessMasterTagSettingDto();
    }

    show(item: GetBusinessMasterTagSettingForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
