import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetBusinessNoteForViewDto, BusinessNoteDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewBusinessNoteModal',
    templateUrl: './view-businessNote-modal.component.html',
})
export class ViewBusinessNoteModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetBusinessNoteForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetBusinessNoteForViewDto();
        this.item.businessNote = new BusinessNoteDto();
    }

    show(item: GetBusinessNoteForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
