import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { LeadNotesServiceProxy, CreateOrEditLeadNoteDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { LeadNoteLeadLookupTableModalComponent } from './leadNote-lead-lookup-table-modal.component';

@Component({
    selector: 'createOrEditLeadNoteModal',
    templateUrl: './create-or-edit-leadNote-modal.component.html',
})
export class CreateOrEditLeadNoteModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('leadNoteLeadLookupTableModal', { static: true })
    leadNoteLeadLookupTableModal: LeadNoteLeadLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    leadNote: CreateOrEditLeadNoteDto = new CreateOrEditLeadNoteDto();

    leadTitle = '';

    constructor(
        injector: Injector,
        private _leadNotesServiceProxy: LeadNotesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(leadNoteId?: number): void {
        if (!leadNoteId) {
            this.leadNote = new CreateOrEditLeadNoteDto();
            this.leadNote.id = leadNoteId;
            this.leadTitle = '';

            this.active = true;
            this.modal.show();
        } else {
            this._leadNotesServiceProxy.getLeadNoteForEdit(leadNoteId).subscribe((result) => {
                this.leadNote = result.leadNote;

                this.leadTitle = result.leadTitle;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._leadNotesServiceProxy
            .createOrEdit(this.leadNote)
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
        this.leadNoteLeadLookupTableModal.id = this.leadNote.leadId;
        this.leadNoteLeadLookupTableModal.displayName = this.leadTitle;
        this.leadNoteLeadLookupTableModal.show();
    }

    setLeadIdNull() {
        this.leadNote.leadId = null;
        this.leadTitle = '';
    }

    getNewLeadId() {
        this.leadNote.leadId = this.leadNoteLeadLookupTableModal.id;
        this.leadTitle = this.leadNoteLeadLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
