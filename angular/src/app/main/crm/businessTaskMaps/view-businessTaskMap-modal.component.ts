import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetBusinessTaskMapForViewDto, BusinessTaskMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewBusinessTaskMapModal',
    templateUrl: './view-businessTaskMap-modal.component.html',
})
export class ViewBusinessTaskMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetBusinessTaskMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetBusinessTaskMapForViewDto();
        this.item.businessTaskMap = new BusinessTaskMapDto();
    }

    show(item: GetBusinessTaskMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
