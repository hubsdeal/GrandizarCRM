import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    StoreTagSettingCategoriesServiceProxy,
    CreateOrEditStoreTagSettingCategoryDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditStoreTagSettingCategoryModal',
    templateUrl: './create-or-edit-storeTagSettingCategory-modal.component.html',
})
export class CreateOrEditStoreTagSettingCategoryModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeTagSettingCategory: CreateOrEditStoreTagSettingCategoryDto = new CreateOrEditStoreTagSettingCategoryDto();

    constructor(
        injector: Injector,
        private _storeTagSettingCategoriesServiceProxy: StoreTagSettingCategoriesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeTagSettingCategoryId?: number): void {
        if (!storeTagSettingCategoryId) {
            this.storeTagSettingCategory = new CreateOrEditStoreTagSettingCategoryDto();
            this.storeTagSettingCategory.id = storeTagSettingCategoryId;

            this.active = true;
            this.modal.show();
        } else {
            this._storeTagSettingCategoriesServiceProxy
                .getStoreTagSettingCategoryForEdit(storeTagSettingCategoryId)
                .subscribe((result) => {
                    this.storeTagSettingCategory = result.storeTagSettingCategory;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._storeTagSettingCategoriesServiceProxy
            .createOrEdit(this.storeTagSettingCategory)
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
