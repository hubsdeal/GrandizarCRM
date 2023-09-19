import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetTaskNoteForViewDto, TaskNoteDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewTaskNoteModal',
    templateUrl: './view-taskNote-modal.component.html'
})
export class ViewTaskNoteModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTaskNoteForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetTaskNoteForViewDto();
        this.item.taskNote = new TaskNoteDto();
    }

    show(item: GetTaskNoteForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
