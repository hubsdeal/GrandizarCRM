import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetLeadTaskForViewDto, LeadTaskDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewLeadTaskModal',
    templateUrl: './view-leadTask-modal.component.html',
})
export class ViewLeadTaskModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetLeadTaskForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetLeadTaskForViewDto();
        this.item.leadTask = new LeadTaskDto();
    }

    show(item: GetLeadTaskForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
