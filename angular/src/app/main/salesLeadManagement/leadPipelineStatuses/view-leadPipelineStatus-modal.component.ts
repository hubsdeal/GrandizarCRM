import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetLeadPipelineStatusForViewDto, LeadPipelineStatusDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewLeadPipelineStatusModal',
    templateUrl: './view-leadPipelineStatus-modal.component.html',
})
export class ViewLeadPipelineStatusModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetLeadPipelineStatusForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetLeadPipelineStatusForViewDto();
        this.item.leadPipelineStatus = new LeadPipelineStatusDto();
    }

    show(item: GetLeadPipelineStatusForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
