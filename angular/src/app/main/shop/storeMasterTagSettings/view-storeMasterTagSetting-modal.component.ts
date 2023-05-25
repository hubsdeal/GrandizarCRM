import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    GetStoreMasterTagSettingForViewDto,
    StoreMasterTagSettingDto,
    AnswerType,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreMasterTagSettingModal',
    templateUrl: './view-storeMasterTagSetting-modal.component.html',
})
export class ViewStoreMasterTagSettingModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreMasterTagSettingForViewDto;
    answerType = AnswerType;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreMasterTagSettingForViewDto();
        this.item.storeMasterTagSetting = new StoreMasterTagSettingDto();
    }

    show(item: GetStoreMasterTagSettingForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
