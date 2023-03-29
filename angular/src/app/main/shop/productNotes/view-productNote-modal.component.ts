import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetProductNoteForViewDto, ProductNoteDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductNoteModal',
    templateUrl: './view-productNote-modal.component.html',
})
export class ViewProductNoteModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductNoteForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductNoteForViewDto();
        this.item.productNote = new ProductNoteDto();
    }

    show(item: GetProductNoteForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
