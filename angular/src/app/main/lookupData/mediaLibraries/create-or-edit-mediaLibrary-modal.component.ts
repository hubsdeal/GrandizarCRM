import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { MediaLibrariesServiceProxy, CreateOrEditMediaLibraryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { MediaLibraryMasterTagCategoryLookupTableModalComponent } from './mediaLibrary-masterTagCategory-lookup-table-modal.component';
import { MediaLibraryMasterTagLookupTableModalComponent } from './mediaLibrary-masterTag-lookup-table-modal.component';

@Component({
    selector: 'createOrEditMediaLibraryModal',
    templateUrl: './create-or-edit-mediaLibrary-modal.component.html',
})
export class CreateOrEditMediaLibraryModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('mediaLibraryMasterTagCategoryLookupTableModal', { static: true })
    mediaLibraryMasterTagCategoryLookupTableModal: MediaLibraryMasterTagCategoryLookupTableModalComponent;
    @ViewChild('mediaLibraryMasterTagLookupTableModal', { static: true })
    mediaLibraryMasterTagLookupTableModal: MediaLibraryMasterTagLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    mediaLibrary: CreateOrEditMediaLibraryDto = new CreateOrEditMediaLibraryDto();

    masterTagCategoryName = '';
    masterTagName = '';

    constructor(
        injector: Injector,
        private _mediaLibrariesServiceProxy: MediaLibrariesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(mediaLibraryId?: number): void {
        if (!mediaLibraryId) {
            this.mediaLibrary = new CreateOrEditMediaLibraryDto();
            this.mediaLibrary.id = mediaLibraryId;
            this.masterTagCategoryName = '';
            this.masterTagName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._mediaLibrariesServiceProxy.getMediaLibraryForEdit(mediaLibraryId).subscribe((result) => {
                this.mediaLibrary = result.mediaLibrary;

                this.masterTagCategoryName = result.masterTagCategoryName;
                this.masterTagName = result.masterTagName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._mediaLibrariesServiceProxy
            .createOrEdit(this.mediaLibrary)
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

    openSelectMasterTagCategoryModal() {
        this.mediaLibraryMasterTagCategoryLookupTableModal.id = this.mediaLibrary.masterTagCategoryId;
        this.mediaLibraryMasterTagCategoryLookupTableModal.displayName = this.masterTagCategoryName;
        this.mediaLibraryMasterTagCategoryLookupTableModal.show();
    }
    openSelectMasterTagModal() {
        this.mediaLibraryMasterTagLookupTableModal.id = this.mediaLibrary.masterTagId;
        this.mediaLibraryMasterTagLookupTableModal.displayName = this.masterTagName;
        this.mediaLibraryMasterTagLookupTableModal.show();
    }

    setMasterTagCategoryIdNull() {
        this.mediaLibrary.masterTagCategoryId = null;
        this.masterTagCategoryName = '';
    }
    setMasterTagIdNull() {
        this.mediaLibrary.masterTagId = null;
        this.masterTagName = '';
    }

    getNewMasterTagCategoryId() {
        this.mediaLibrary.masterTagCategoryId = this.mediaLibraryMasterTagCategoryLookupTableModal.id;
        this.masterTagCategoryName = this.mediaLibraryMasterTagCategoryLookupTableModal.displayName;
    }
    getNewMasterTagId() {
        this.mediaLibrary.masterTagId = this.mediaLibraryMasterTagLookupTableModal.id;
        this.masterTagName = this.mediaLibraryMasterTagLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
