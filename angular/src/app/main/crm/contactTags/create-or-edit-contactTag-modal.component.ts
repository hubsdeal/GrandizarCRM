import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ContactTagsServiceProxy, CreateOrEditContactTagDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ContactTagContactLookupTableModalComponent } from './contactTag-contact-lookup-table-modal.component';
import { ContactTagMasterTagCategoryLookupTableModalComponent } from './contactTag-masterTagCategory-lookup-table-modal.component';
import { ContactTagMasterTagLookupTableModalComponent } from './contactTag-masterTag-lookup-table-modal.component';

@Component({
    selector: 'createOrEditContactTagModal',
    templateUrl: './create-or-edit-contactTag-modal.component.html',
})
export class CreateOrEditContactTagModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('contactTagContactLookupTableModal', { static: true })
    contactTagContactLookupTableModal: ContactTagContactLookupTableModalComponent;
    @ViewChild('contactTagMasterTagCategoryLookupTableModal', { static: true })
    contactTagMasterTagCategoryLookupTableModal: ContactTagMasterTagCategoryLookupTableModalComponent;
    @ViewChild('contactTagMasterTagLookupTableModal', { static: true })
    contactTagMasterTagLookupTableModal: ContactTagMasterTagLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    contactTag: CreateOrEditContactTagDto = new CreateOrEditContactTagDto();

    contactFullName = '';
    masterTagCategoryName = '';
    masterTagName = '';

    constructor(
        injector: Injector,
        private _contactTagsServiceProxy: ContactTagsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(contactTagId?: number): void {
        if (!contactTagId) {
            this.contactTag = new CreateOrEditContactTagDto();
            this.contactTag.id = contactTagId;
            this.contactFullName = '';
            this.masterTagCategoryName = '';
            this.masterTagName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._contactTagsServiceProxy.getContactTagForEdit(contactTagId).subscribe((result) => {
                this.contactTag = result.contactTag;

                this.contactFullName = result.contactFullName;
                this.masterTagCategoryName = result.masterTagCategoryName;
                this.masterTagName = result.masterTagName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._contactTagsServiceProxy
            .createOrEdit(this.contactTag)
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

    openSelectContactModal() {
        this.contactTagContactLookupTableModal.id = this.contactTag.contactId;
        this.contactTagContactLookupTableModal.displayName = this.contactFullName;
        this.contactTagContactLookupTableModal.show();
    }
    openSelectMasterTagCategoryModal() {
        this.contactTagMasterTagCategoryLookupTableModal.id = this.contactTag.masterTagCategoryId;
        this.contactTagMasterTagCategoryLookupTableModal.displayName = this.masterTagCategoryName;
        this.contactTagMasterTagCategoryLookupTableModal.show();
    }
    openSelectMasterTagModal() {
        this.contactTagMasterTagLookupTableModal.id = this.contactTag.masterTagId;
        this.contactTagMasterTagLookupTableModal.displayName = this.masterTagName;
        this.contactTagMasterTagLookupTableModal.show();
    }

    setContactIdNull() {
        this.contactTag.contactId = null;
        this.contactFullName = '';
    }
    setMasterTagCategoryIdNull() {
        this.contactTag.masterTagCategoryId = null;
        this.masterTagCategoryName = '';
    }
    setMasterTagIdNull() {
        this.contactTag.masterTagId = null;
        this.masterTagName = '';
    }

    getNewContactId() {
        this.contactTag.contactId = this.contactTagContactLookupTableModal.id;
        this.contactFullName = this.contactTagContactLookupTableModal.displayName;
    }
    getNewMasterTagCategoryId() {
        this.contactTag.masterTagCategoryId = this.contactTagMasterTagCategoryLookupTableModal.id;
        this.masterTagCategoryName = this.contactTagMasterTagCategoryLookupTableModal.displayName;
    }
    getNewMasterTagId() {
        this.contactTag.masterTagId = this.contactTagMasterTagLookupTableModal.id;
        this.masterTagName = this.contactTagMasterTagLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
