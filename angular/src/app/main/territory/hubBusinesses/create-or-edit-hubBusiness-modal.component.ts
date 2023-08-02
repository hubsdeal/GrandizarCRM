import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { HubBusinessesServiceProxy, CreateOrEditHubBusinessDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { HubBusinessHubLookupTableModalComponent } from './hubBusiness-hub-lookup-table-modal.component';
import { HubBusinessBusinessLookupTableModalComponent } from './hubBusiness-business-lookup-table-modal.component';

@Component({
    selector: 'createOrEditHubBusinessModal',
    templateUrl: './create-or-edit-hubBusiness-modal.component.html',
})
export class CreateOrEditHubBusinessModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('hubBusinessHubLookupTableModal', { static: true })
    hubBusinessHubLookupTableModal: HubBusinessHubLookupTableModalComponent;
    @ViewChild('hubBusinessBusinessLookupTableModal', { static: true })
    hubBusinessBusinessLookupTableModal: HubBusinessBusinessLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    hubId:number

    hubBusiness: CreateOrEditHubBusinessDto = new CreateOrEditHubBusinessDto();

    hubName = '';
    businessName = '';

    constructor(
        injector: Injector,
        private _hubBusinessesServiceProxy: HubBusinessesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(hubBusinessId?: number): void {
        if (!hubBusinessId) {
            this.hubBusiness = new CreateOrEditHubBusinessDto();
            this.hubBusiness.id = hubBusinessId;
            this.hubName = '';
            this.businessName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._hubBusinessesServiceProxy.getHubBusinessForEdit(hubBusinessId).subscribe((result) => {
                this.hubBusiness = result.hubBusiness;
                this.hubId = result.hubBusiness.hubId;
                this.hubName = result.hubName;
                this.businessName = result.businessName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this.hubBusiness.hubId = this.hubId;
        this._hubBusinessesServiceProxy
            .createOrEdit(this.hubBusiness)
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

    openSelectHubModal() {
        this.hubBusinessHubLookupTableModal.id = this.hubBusiness.hubId;
        this.hubBusinessHubLookupTableModal.displayName = this.hubName;
        this.hubBusinessHubLookupTableModal.show();
    }
    openSelectBusinessModal() {
        this.hubBusinessBusinessLookupTableModal.id = this.hubBusiness.businessId;
        this.hubBusinessBusinessLookupTableModal.displayName = this.businessName;
        this.hubBusinessBusinessLookupTableModal.show();
    }

    setHubIdNull() {
        this.hubBusiness.hubId = null;
        this.hubName = '';
    }
    setBusinessIdNull() {
        this.hubBusiness.businessId = null;
        this.businessName = '';
    }

    getNewHubId() {
        this.hubBusiness.hubId = this.hubBusinessHubLookupTableModal.id;
        this.hubName = this.hubBusinessHubLookupTableModal.displayName;
    }
    getNewBusinessId() {
        this.hubBusiness.businessId = this.hubBusinessBusinessLookupTableModal.id;
        this.businessName = this.hubBusinessBusinessLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
