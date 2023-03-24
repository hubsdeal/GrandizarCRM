import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetTaskStatusForViewDto, TaskStatusDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewTaskStatusModal',
    templateUrl: './view-taskStatus-modal.component.html',
})
export class ViewTaskStatusModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTaskStatusForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetTaskStatusForViewDto();
        this.item.taskStatus = new TaskStatusDto();
    }

    show(item: GetTaskStatusForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
