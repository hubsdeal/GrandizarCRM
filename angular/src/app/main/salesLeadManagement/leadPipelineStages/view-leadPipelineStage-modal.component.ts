import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetLeadPipelineStageForViewDto, LeadPipelineStageDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewLeadPipelineStageModal',
    templateUrl: './view-leadPipelineStage-modal.component.html',
})
export class ViewLeadPipelineStageModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetLeadPipelineStageForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetLeadPipelineStageForViewDto();
        this.item.leadPipelineStage = new LeadPipelineStageDto();
    }

    show(item: GetLeadPipelineStageForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
