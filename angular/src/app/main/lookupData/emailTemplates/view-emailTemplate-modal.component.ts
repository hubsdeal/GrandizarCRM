import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetEmailTemplateForViewDto, EmailTemplateDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewEmailTemplateModal',
    templateUrl: './view-emailTemplate-modal.component.html',
})
export class ViewEmailTemplateModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmailTemplateForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetEmailTemplateForViewDto();
        this.item.emailTemplate = new EmailTemplateDto();
    }

    show(item: GetEmailTemplateForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
