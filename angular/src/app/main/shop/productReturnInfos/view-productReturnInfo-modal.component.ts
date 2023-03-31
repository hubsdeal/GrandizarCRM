import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetProductReturnInfoForViewDto, ProductReturnInfoDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductReturnInfoModal',
    templateUrl: './view-productReturnInfo-modal.component.html',
})
export class ViewProductReturnInfoModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductReturnInfoForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductReturnInfoForViewDto();
        this.item.productReturnInfo = new ProductReturnInfoDto();
    }

    show(item: GetProductReturnInfoForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
