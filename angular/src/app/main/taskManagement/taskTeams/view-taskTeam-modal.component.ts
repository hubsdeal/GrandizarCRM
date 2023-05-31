import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetTaskTeamForViewDto, TaskTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewTaskTeamModal',
    templateUrl: './view-taskTeam-modal.component.html',
})
export class ViewTaskTeamModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTaskTeamForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetTaskTeamForViewDto();
        this.item.taskTeam = new TaskTeamDto();
    }

    show(item: GetTaskTeamForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
