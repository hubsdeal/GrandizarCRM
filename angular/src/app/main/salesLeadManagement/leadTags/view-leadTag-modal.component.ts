import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetLeadTagForViewDto, LeadTagDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewLeadTagModal',
    templateUrl: './view-leadTag-modal.component.html',
})
export class ViewLeadTagModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetLeadTagForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetLeadTagForViewDto();
        this.item.leadTag = new LeadTagDto();
    }

    show(item: GetLeadTagForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
