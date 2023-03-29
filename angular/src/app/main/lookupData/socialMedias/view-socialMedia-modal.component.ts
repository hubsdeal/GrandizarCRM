import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetSocialMediaForViewDto, SocialMediaDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewSocialMediaModal',
    templateUrl: './view-socialMedia-modal.component.html',
})
export class ViewSocialMediaModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetSocialMediaForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetSocialMediaForViewDto();
        this.item.socialMedia = new SocialMediaDto();
    }

    show(item: GetSocialMediaForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
