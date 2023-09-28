import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetTaskManagerRatingForViewDto, TaskManagerRatingDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewTaskManagerRatingModal',
    templateUrl: './view-taskManagerRating-modal.component.html'
})
export class ViewTaskManagerRatingModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTaskManagerRatingForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetTaskManagerRatingForViewDto();
        this.item.taskManagerRating = new TaskManagerRatingDto();
    }

    show(item: GetTaskManagerRatingForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
