﻿import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetContactCertificationLicenseForViewDto, ContactCertificationLicenseDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewContactCertificationLicenseModal',
    templateUrl: './view-contactCertificationLicense-modal.component.html'
})
export class ViewContactCertificationLicenseModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetContactCertificationLicenseForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetContactCertificationLicenseForViewDto();
        this.item.contactCertificationLicense = new ContactCertificationLicenseDto();
    }

    show(item: GetContactCertificationLicenseForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
