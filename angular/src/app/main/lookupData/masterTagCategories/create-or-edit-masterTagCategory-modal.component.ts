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

@Component({
    selector: 'createOrEditMasterTagCategoryModal',
    templateUrl: './create-or-edit-masterTagCategory-modal.component.html',
})
export class CreateOrEditMasterTagCategoryModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    masterTagCategory: CreateOrEditMasterTagCategoryDto = new CreateOrEditMasterTagCategoryDto();

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

            this.active = true;
            this.modal.show();
        } else {
            this._masterTagCategoriesServiceProxy
                .getMasterTagCategoryForEdit(masterTagCategoryId)
                .subscribe((result) => {
                    this.masterTagCategory = result.masterTagCategory;

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

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
