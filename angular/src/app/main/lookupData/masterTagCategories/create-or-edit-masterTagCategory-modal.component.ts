import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    MasterTagCategoriesServiceProxy,
    CreateOrEditMasterTagCategoryDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { MasterTagCategoryMediaLibraryLookupTableModalComponent } from './masterTagCategory-mediaLibrary-lookup-table-modal.component';

@Component({
    selector: 'createOrEditMasterTagCategoryModal',
    templateUrl: './create-or-edit-masterTagCategory-modal.component.html',
})
export class CreateOrEditMasterTagCategoryModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('masterTagCategoryMediaLibraryLookupTableModal', { static: true })
    masterTagCategoryMediaLibraryLookupTableModal: MasterTagCategoryMediaLibraryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    masterTagCategory: CreateOrEditMasterTagCategoryDto = new CreateOrEditMasterTagCategoryDto();

    mediaLibraryName = '';

    constructor(
        injector: Injector,
        private _masterTagCategoriesServiceProxy: MasterTagCategoriesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(masterTagCategoryId?: number): void {
        if (!masterTagCategoryId) {
            this.masterTagCategory = new CreateOrEditMasterTagCategoryDto();
            this.masterTagCategory.id = masterTagCategoryId;
            this.mediaLibraryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._masterTagCategoriesServiceProxy
                .getMasterTagCategoryForEdit(masterTagCategoryId)
                .subscribe((result) => {
                    this.masterTagCategory = result.masterTagCategory;

                    this.mediaLibraryName = result.mediaLibraryName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._masterTagCategoriesServiceProxy
            .createOrEdit(this.masterTagCategory)
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

    openSelectMediaLibraryModal() {
        this.masterTagCategoryMediaLibraryLookupTableModal.id = this.masterTagCategory.pictureMediaLibraryId;
        this.masterTagCategoryMediaLibraryLookupTableModal.displayName = this.mediaLibraryName;
        this.masterTagCategoryMediaLibraryLookupTableModal.show();
    }

    setPictureMediaLibraryIdNull() {
        this.masterTagCategory.pictureMediaLibraryId = null;
        this.mediaLibraryName = '';
    }

    getNewPictureMediaLibraryId() {
        this.masterTagCategory.pictureMediaLibraryId = this.masterTagCategoryMediaLibraryLookupTableModal.id;
        this.mediaLibraryName = this.masterTagCategoryMediaLibraryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
