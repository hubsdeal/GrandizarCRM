import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetTaskWorkItemForViewDto, TaskWorkItemDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewTaskWorkItemModal',
    templateUrl: './view-taskWorkItem-modal.component.html',
})
export class ViewTaskWorkItemModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTaskWorkItemForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetTaskWorkItemForViewDto();
        this.item.taskWorkItem = new TaskWorkItemDto();
    }

    show(item: GetTaskWorkItemForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
