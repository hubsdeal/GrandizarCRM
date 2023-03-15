import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetMembershipTypeForViewDto, MembershipTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewMembershipTypeModal',
    templateUrl: './view-membershipType-modal.component.html',
})
export class ViewMembershipTypeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetMembershipTypeForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetMembershipTypeForViewDto();
        this.item.membershipType = new MembershipTypeDto();
    }

    show(item: GetMembershipTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
