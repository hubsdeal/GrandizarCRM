import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ContactsServiceProxy,
    CreateOrEditContactDto,
    ContactCountryLookupTableDto,
    ContactStateLookupTableDto,
    ContactMembershipTypeLookupTableDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ContactUserLookupTableModalComponent } from './contact-user-lookup-table-modal.component';

@Component({
    selector: 'createOrEditContactModal',
    templateUrl: './create-or-edit-contact-modal.component.html',
})
export class CreateOrEditContactModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('contactUserLookupTableModal', { static: true })
    contactUserLookupTableModal: ContactUserLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    contact: CreateOrEditContactDto = new CreateOrEditContactDto();

    userName = '';
    countryName = '';
    stateName = '';
    membershipTypeName = '';

    allCountrys: ContactCountryLookupTableDto[];
    allStates: ContactStateLookupTableDto[];
    allMembershipTypes: ContactMembershipTypeLookupTableDto[];

    constructor(
        injector: Injector,
        private _contactsServiceProxy: ContactsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(contactId?: number): void {
        if (!contactId) {
            this.contact = new CreateOrEditContactDto();
            this.contact.id = contactId;
            this.contact.dateOfBirth = this._dateTimeService.getStartOfDay();
            this.userName = '';
            this.countryName = '';
            this.stateName = '';
            this.membershipTypeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._contactsServiceProxy.getContactForEdit(contactId).subscribe((result) => {
                this.contact = result.contact;

                this.userName = result.userName;
                this.countryName = result.countryName;
                this.stateName = result.stateName;
                this.membershipTypeName = result.membershipTypeName;

                this.active = true;
                this.modal.show();
            });
        }
        this._contactsServiceProxy.getAllCountryForTableDropdown().subscribe((result) => {
            this.allCountrys = result;
        });
        this._contactsServiceProxy.getAllStateForTableDropdown().subscribe((result) => {
            this.allStates = result;
        });
        this._contactsServiceProxy.getAllMembershipTypeForTableDropdown().subscribe((result) => {
            this.allMembershipTypes = result;
        });
    }

    save(): void {
        this.saving = true;

        this._contactsServiceProxy
            .createOrEdit(this.contact)
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

    openSelectUserModal() {
        this.contactUserLookupTableModal.id = this.contact.referredByUserId;
        this.contactUserLookupTableModal.displayName = this.userName;
        this.contactUserLookupTableModal.show();
    }

    setReferredByUserIdNull() {
        this.contact.referredByUserId = null;
        this.userName = '';
    }

    getNewReferredByUserId() {
        this.contact.referredByUserId = this.contactUserLookupTableModal.id;
        this.userName = this.contactUserLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
