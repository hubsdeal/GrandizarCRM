import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { LeadContactsServiceProxy, CreateOrEditLeadContactDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { LeadContactLeadLookupTableModalComponent } from './leadContact-lead-lookup-table-modal.component';
import { LeadContactContactLookupTableModalComponent } from './leadContact-contact-lookup-table-modal.component';

@Component({
    selector: 'createOrEditLeadContactModal',
    templateUrl: './create-or-edit-leadContact-modal.component.html',
})
export class CreateOrEditLeadContactModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('leadContactLeadLookupTableModal', { static: true })
    leadContactLeadLookupTableModal: LeadContactLeadLookupTableModalComponent;
    @ViewChild('leadContactContactLookupTableModal', { static: true })
    leadContactContactLookupTableModal: LeadContactContactLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    leadContact: CreateOrEditLeadContactDto = new CreateOrEditLeadContactDto();

    leadTitle = '';
    contactFullName = '';

    constructor(
        injector: Injector,
        private _leadContactsServiceProxy: LeadContactsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(leadContactId?: number): void {
        if (!leadContactId) {
            this.leadContact = new CreateOrEditLeadContactDto();
            this.leadContact.id = leadContactId;
            this.leadTitle = '';
            this.contactFullName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._leadContactsServiceProxy.getLeadContactForEdit(leadContactId).subscribe((result) => {
                this.leadContact = result.leadContact;

                this.leadTitle = result.leadTitle;
                this.contactFullName = result.contactFullName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._leadContactsServiceProxy
            .createOrEdit(this.leadContact)
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

    openSelectLeadModal() {
        this.leadContactLeadLookupTableModal.id = this.leadContact.leadId;
        this.leadContactLeadLookupTableModal.displayName = this.leadTitle;
        this.leadContactLeadLookupTableModal.show();
    }
    openSelectContactModal() {
        this.leadContactContactLookupTableModal.id = this.leadContact.contactId;
        this.leadContactContactLookupTableModal.displayName = this.contactFullName;
        this.leadContactContactLookupTableModal.show();
    }

    setLeadIdNull() {
        this.leadContact.leadId = null;
        this.leadTitle = '';
    }
    setContactIdNull() {
        this.leadContact.contactId = null;
        this.contactFullName = '';
    }

    getNewLeadId() {
        this.leadContact.leadId = this.leadContactLeadLookupTableModal.id;
        this.leadTitle = this.leadContactLeadLookupTableModal.displayName;
    }
    getNewContactId() {
        this.leadContact.contactId = this.leadContactContactLookupTableModal.id;
        this.contactFullName = this.leadContactContactLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
