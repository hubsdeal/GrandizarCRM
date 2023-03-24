import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { BusinessNotesServiceProxy, CreateOrEditBusinessNoteDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { BusinessNoteBusinessLookupTableModalComponent } from './businessNote-business-lookup-table-modal.component';

@Component({
    selector: 'createOrEditBusinessNoteModal',
    templateUrl: './create-or-edit-businessNote-modal.component.html',
})
export class CreateOrEditBusinessNoteModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('businessNoteBusinessLookupTableModal', { static: true })
    businessNoteBusinessLookupTableModal: BusinessNoteBusinessLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    businessNote: CreateOrEditBusinessNoteDto = new CreateOrEditBusinessNoteDto();

    businessName = '';

    constructor(
        injector: Injector,
        private _businessNotesServiceProxy: BusinessNotesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(businessNoteId?: number): void {
        if (!businessNoteId) {
            this.businessNote = new CreateOrEditBusinessNoteDto();
            this.businessNote.id = businessNoteId;
            this.businessName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._businessNotesServiceProxy.getBusinessNoteForEdit(businessNoteId).subscribe((result) => {
                this.businessNote = result.businessNote;

                this.businessName = result.businessName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._businessNotesServiceProxy
            .createOrEdit(this.businessNote)
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
        this.businessNoteBusinessLookupTableModal.id = this.businessNote.businessId;
        this.businessNoteBusinessLookupTableModal.displayName = this.businessName;
        this.businessNoteBusinessLookupTableModal.show();
    }

    setBusinessIdNull() {
        this.businessNote.businessId = null;
        this.businessName = '';
    }

    getNewBusinessId() {
        this.businessNote.businessId = this.businessNoteBusinessLookupTableModal.id;
        this.businessName = this.businessNoteBusinessLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
