import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    GetContactMasterTagSettingForViewDto,
    ContactMasterTagSettingDto,
    AnswerType,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewContactMasterTagSettingModal',
    templateUrl: './view-contactMasterTagSetting-modal.component.html',
})
export class ViewContactMasterTagSettingModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetContactMasterTagSettingForViewDto;
    answerType = AnswerType;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetContactMasterTagSettingForViewDto();
        this.item.contactMasterTagSetting = new ContactMasterTagSettingDto();
    }

    show(item: GetContactMasterTagSettingForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
