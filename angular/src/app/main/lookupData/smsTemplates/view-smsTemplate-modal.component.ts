import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetSmsTemplateForViewDto, SmsTemplateDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewSmsTemplateModal',
    templateUrl: './view-smsTemplate-modal.component.html',
})
export class ViewSmsTemplateModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetSmsTemplateForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetSmsTemplateForViewDto();
        this.item.smsTemplate = new SmsTemplateDto();
    }

    show(item: GetSmsTemplateForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
