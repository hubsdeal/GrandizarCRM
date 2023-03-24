import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreMediasServiceProxy, CreateOrEditStoreMediaDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreMediaStoreLookupTableModalComponent } from './storeMedia-store-lookup-table-modal.component';
import { StoreMediaMediaLibraryLookupTableModalComponent } from './storeMedia-mediaLibrary-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreMediaModal',
    templateUrl: './create-or-edit-storeMedia-modal.component.html',
})
export class CreateOrEditStoreMediaModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeMediaStoreLookupTableModal', { static: true })
    storeMediaStoreLookupTableModal: StoreMediaStoreLookupTableModalComponent;
    @ViewChild('storeMediaMediaLibraryLookupTableModal', { static: true })
    storeMediaMediaLibraryLookupTableModal: StoreMediaMediaLibraryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeMedia: CreateOrEditStoreMediaDto = new CreateOrEditStoreMediaDto();

    storeName = '';
    mediaLibraryName = '';

    constructor(
        injector: Injector,
        private _storeMediasServiceProxy: StoreMediasServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeMediaId?: number): void {
        if (!storeMediaId) {
            this.storeMedia = new CreateOrEditStoreMediaDto();
            this.storeMedia.id = storeMediaId;
            this.storeName = '';
            this.mediaLibraryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeMediasServiceProxy.getStoreMediaForEdit(storeMediaId).subscribe((result) => {
                this.storeMedia = result.storeMedia;

                this.storeName = result.storeName;
                this.mediaLibraryName = result.mediaLibraryName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._storeMediasServiceProxy
            .createOrEdit(this.storeMedia)
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
        this.storeMediaStoreLookupTableModal.id = this.storeMedia.storeId;
        this.storeMediaStoreLookupTableModal.displayName = this.storeName;
        this.storeMediaStoreLookupTableModal.show();
    }
    openSelectMediaLibraryModal() {
        this.storeMediaMediaLibraryLookupTableModal.id = this.storeMedia.mediaLibraryId;
        this.storeMediaMediaLibraryLookupTableModal.displayName = this.mediaLibraryName;
        this.storeMediaMediaLibraryLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeMedia.storeId = null;
        this.storeName = '';
    }
    setMediaLibraryIdNull() {
        this.storeMedia.mediaLibraryId = null;
        this.mediaLibraryName = '';
    }

    getNewStoreId() {
        this.storeMedia.storeId = this.storeMediaStoreLookupTableModal.id;
        this.storeName = this.storeMediaStoreLookupTableModal.displayName;
    }
    getNewMediaLibraryId() {
        this.storeMedia.mediaLibraryId = this.storeMediaMediaLibraryLookupTableModal.id;
        this.mediaLibraryName = this.storeMediaMediaLibraryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
