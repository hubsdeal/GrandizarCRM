import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    BusinessContactMapsServiceProxy,
    CreateOrEditBusinessContactMapDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { BusinessContactMapBusinessLookupTableModalComponent } from './businessContactMap-business-lookup-table-modal.component';
import { BusinessContactMapContactLookupTableModalComponent } from './businessContactMap-contact-lookup-table-modal.component';

@Component({
    selector: 'createOrEditBusinessContactMapModal',
    templateUrl: './create-or-edit-businessContactMap-modal.component.html',
})
export class CreateOrEditBusinessContactMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('businessContactMapBusinessLookupTableModal', { static: true })
    businessContactMapBusinessLookupTableModal: BusinessContactMapBusinessLookupTableModalComponent;
    @ViewChild('businessContactMapContactLookupTableModal', { static: true })
    businessContactMapContactLookupTableModal: BusinessContactMapContactLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    businessContactMap: CreateOrEditBusinessContactMapDto = new CreateOrEditBusinessContactMapDto();

    businessName = '';
    contactFullName = '';

    constructor(
        injector: Injector,
        private _businessContactMapsServiceProxy: BusinessContactMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(businessContactMapId?: number): void {
        if (!businessContactMapId) {
            this.businessContactMap = new CreateOrEditBusinessContactMapDto();
            this.businessContactMap.id = businessContactMapId;
            this.businessName = '';
            this.contactFullName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._businessContactMapsServiceProxy
                .getBusinessContactMapForEdit(businessContactMapId)
                .subscribe((result) => {
                    this.businessContactMap = result.businessContactMap;

                    this.businessName = result.businessName;
                    this.contactFullName = result.contactFullName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._businessContactMapsServiceProxy
            .createOrEdit(this.businessContactMap)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    openSelectBusinessModal() {
        this.businessContactMapBusinessLookupTableModal.id = this.businessContactMap.businessId;
        this.businessContactMapBusinessLookupTableModal.displayName = this.businessName;
        this.businessContactMapBusinessLookupTableModal.show();
    }
    openSelectContactModal() {
        this.businessContactMapContactLookupTableModal.id = this.businessContactMap.contactId;
        this.businessContactMapContactLookupTableModal.displayName = this.contactFullName;
        this.businessContactMapContactLookupTableModal.show();
    }

    setBusinessIdNull() {
        this.businessContactMap.businessId = null;
        this.businessName = '';
    }
    setContactIdNull() {
        this.businessContactMap.contactId = null;
        this.contactFullName = '';
    }

    getNewBusinessId() {
        this.businessContactMap.businessId = this.businessContactMapBusinessLookupTableModal.id;
        this.businessName = this.businessContactMapBusinessLookupTableModal.displayName;
    }
    getNewContactId() {
        this.businessContactMap.contactId = this.businessContactMapContactLookupTableModal.id;
        this.contactFullName = this.businessContactMapContactLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
