import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    GetProductOwnerPublicContactInfoForViewDto,
    ProductOwnerPublicContactInfoDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductOwnerPublicContactInfoModal',
    templateUrl: './view-productOwnerPublicContactInfo-modal.component.html',
})
export class ViewProductOwnerPublicContactInfoModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductOwnerPublicContactInfoForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductOwnerPublicContactInfoForViewDto();
        this.item.productOwnerPublicContactInfo = new ProductOwnerPublicContactInfoDto();
    }

    show(item: GetProductOwnerPublicContactInfoForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
