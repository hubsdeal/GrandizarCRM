import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetTaskEventForViewDto, TaskEventDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewTaskEventModal',
    templateUrl: './view-taskEvent-modal.component.html',
})
export class ViewTaskEventModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTaskEventForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetTaskEventForViewDto();
        this.item.taskEvent = new TaskEventDto();
    }

    show(item: GetTaskEventForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
