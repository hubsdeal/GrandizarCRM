import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    MasterTagsServiceProxy,
    CreateOrEditMasterTagDto,
    MasterTagMasterTagCategoryLookupTableDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditMasterTagModal',
    templateUrl: './create-or-edit-masterTag-modal.component.html',
})
export class CreateOrEditMasterTagModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    masterTag: CreateOrEditMasterTagDto = new CreateOrEditMasterTagDto();

    masterTagCategoryName = '';

    allMasterTagCategorys: MasterTagMasterTagCategoryLookupTableDto[];

    constructor(
        injector: Injector,
        private _masterTagsServiceProxy: MasterTagsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(masterTagId?: number): void {
        if (!masterTagId) {
            this.masterTag = new CreateOrEditMasterTagDto();
            this.masterTag.id = masterTagId;
            this.masterTagCategoryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._masterTagsServiceProxy.getMasterTagForEdit(masterTagId).subscribe((result) => {
                this.masterTag = result.masterTag;

                this.masterTagCategoryName = result.masterTagCategoryName;

                this.active = true;
                this.modal.show();
            });
        }
        this._masterTagsServiceProxy.getAllMasterTagCategoryForTableDropdown().subscribe((result) => {
            this.allMasterTagCategorys = result;
        });
    }

    save(): void {
        this.saving = true;

        this._masterTagsServiceProxy
            .createOrEdit(this.masterTag)
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

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
