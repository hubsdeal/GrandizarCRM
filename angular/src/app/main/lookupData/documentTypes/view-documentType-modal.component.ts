import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetDocumentTypeForViewDto, DocumentTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewDocumentTypeModal',
    templateUrl: './view-documentType-modal.component.html',
})
export class ViewDocumentTypeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetDocumentTypeForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetDocumentTypeForViewDto();
        this.item.documentType = new DocumentTypeDto();
    }

    show(item: GetDocumentTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
