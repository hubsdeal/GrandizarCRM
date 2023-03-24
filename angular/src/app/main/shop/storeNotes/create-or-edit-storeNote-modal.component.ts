import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreNotesServiceProxy, CreateOrEditStoreNoteDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreNoteStoreLookupTableModalComponent } from './storeNote-store-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreNoteModal',
    templateUrl: './create-or-edit-storeNote-modal.component.html',
})
export class CreateOrEditStoreNoteModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeNoteStoreLookupTableModal', { static: true })
    storeNoteStoreLookupTableModal: StoreNoteStoreLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeNote: CreateOrEditStoreNoteDto = new CreateOrEditStoreNoteDto();

    storeName = '';

    constructor(
        injector: Injector,
        private _storeNotesServiceProxy: StoreNotesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeNoteId?: number): void {
        if (!storeNoteId) {
            this.storeNote = new CreateOrEditStoreNoteDto();
            this.storeNote.id = storeNoteId;
            this.storeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeNotesServiceProxy.getStoreNoteForEdit(storeNoteId).subscribe((result) => {
                this.storeNote = result.storeNote;

                this.storeName = result.storeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._storeNotesServiceProxy
            .createOrEdit(this.storeNote)
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

    openSelectStoreModal() {
        this.storeNoteStoreLookupTableModal.id = this.storeNote.storeId;
        this.storeNoteStoreLookupTableModal.displayName = this.storeName;
        this.storeNoteStoreLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeNote.storeId = null;
        this.storeName = '';
    }

    getNewStoreId() {
        this.storeNote.storeId = this.storeNoteStoreLookupTableModal.id;
        this.storeName = this.storeNoteStoreLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
