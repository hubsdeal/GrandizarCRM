import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { HubContactsServiceProxy, CreateOrEditHubContactDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { HubContactHubLookupTableModalComponent } from './hubContact-hub-lookup-table-modal.component';
import { HubContactContactLookupTableModalComponent } from './hubContact-contact-lookup-table-modal.component';

@Component({
    selector: 'createOrEditHubContactModal',
    templateUrl: './create-or-edit-hubContact-modal.component.html',
})
export class CreateOrEditHubContactModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('hubContactHubLookupTableModal', { static: true })
    hubContactHubLookupTableModal: HubContactHubLookupTableModalComponent;
    @ViewChild('hubContactContactLookupTableModal', { static: true })
    hubContactContactLookupTableModal: HubContactContactLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    hubContact: CreateOrEditHubContactDto = new CreateOrEditHubContactDto();

    hubName = '';
    contactFullName = '';

    constructor(
        injector: Injector,
        private _hubContactsServiceProxy: HubContactsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(hubContactId?: number): void {
        if (!hubContactId) {
            this.hubContact = new CreateOrEditHubContactDto();
            this.hubContact.id = hubContactId;
            this.hubName = '';
            this.contactFullName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._hubContactsServiceProxy.getHubContactForEdit(hubContactId).subscribe((result) => {
                this.hubContact = result.hubContact;

                this.hubName = result.hubName;
                this.contactFullName = result.contactFullName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._hubContactsServiceProxy
            .createOrEdit(this.hubContact)
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
        this.hubContactHubLookupTableModal.id = this.hubContact.hubId;
        this.hubContactHubLookupTableModal.displayName = this.hubName;
        this.hubContactHubLookupTableModal.show();
    }
    openSelectContactModal() {
        this.hubContactContactLookupTableModal.id = this.hubContact.contactId;
        this.hubContactContactLookupTableModal.displayName = this.contactFullName;
        this.hubContactContactLookupTableModal.show();
    }

    setHubIdNull() {
        this.hubContact.hubId = null;
        this.hubName = '';
    }
    setContactIdNull() {
        this.hubContact.contactId = null;
        this.contactFullName = '';
    }

    getNewHubId() {
        this.hubContact.hubId = this.hubContactHubLookupTableModal.id;
        this.hubName = this.hubContactHubLookupTableModal.displayName;
    }
    getNewContactId() {
        this.hubContact.contactId = this.hubContactContactLookupTableModal.id;
        this.contactFullName = this.hubContactContactLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
