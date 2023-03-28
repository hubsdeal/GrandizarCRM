import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetHubProductCategoryForViewDto, HubProductCategoryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewHubProductCategoryModal',
    templateUrl: './view-hubProductCategory-modal.component.html',
})
export class ViewHubProductCategoryModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetHubProductCategoryForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetHubProductCategoryForViewDto();
        this.item.hubProductCategory = new HubProductCategoryDto();
    }

    show(item: GetHubProductCategoryForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
