import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { BusinessTagsServiceProxy, CreateOrEditBusinessTagDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { BusinessTagBusinessLookupTableModalComponent } from './businessTag-business-lookup-table-modal.component';
import { BusinessTagMasterTagCategoryLookupTableModalComponent } from './businessTag-masterTagCategory-lookup-table-modal.component';
import { BusinessTagMasterTagLookupTableModalComponent } from './businessTag-masterTag-lookup-table-modal.component';

@Component({
    selector: 'createOrEditBusinessTagModal',
    templateUrl: './create-or-edit-businessTag-modal.component.html',
})
export class CreateOrEditBusinessTagModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('businessTagBusinessLookupTableModal', { static: true })
    businessTagBusinessLookupTableModal: BusinessTagBusinessLookupTableModalComponent;
    @ViewChild('businessTagMasterTagCategoryLookupTableModal', { static: true })
    businessTagMasterTagCategoryLookupTableModal: BusinessTagMasterTagCategoryLookupTableModalComponent;
    @ViewChild('businessTagMasterTagLookupTableModal', { static: true })
    businessTagMasterTagLookupTableModal: BusinessTagMasterTagLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    businessTag: CreateOrEditBusinessTagDto = new CreateOrEditBusinessTagDto();

    businessName = '';
    masterTagCategoryName = '';
    masterTagName = '';

    constructor(
        injector: Injector,
        private _businessTagsServiceProxy: BusinessTagsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(businessTagId?: number): void {
        if (!businessTagId) {
            this.businessTag = new CreateOrEditBusinessTagDto();
            this.businessTag.id = businessTagId;
            this.businessName = '';
            this.masterTagCategoryName = '';
            this.masterTagName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._businessTagsServiceProxy.getBusinessTagForEdit(businessTagId).subscribe((result) => {
                this.businessTag = result.businessTag;

                this.businessName = result.businessName;
                this.masterTagCategoryName = result.masterTagCategoryName;
                this.masterTagName = result.masterTagName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._businessTagsServiceProxy
            .createOrEdit(this.businessTag)
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
        this.businessTagBusinessLookupTableModal.id = this.businessTag.businessId;
        this.businessTagBusinessLookupTableModal.displayName = this.businessName;
        this.businessTagBusinessLookupTableModal.show();
    }
    openSelectMasterTagCategoryModal() {
        this.businessTagMasterTagCategoryLookupTableModal.id = this.businessTag.masterTagCategoryId;
        this.businessTagMasterTagCategoryLookupTableModal.displayName = this.masterTagCategoryName;
        this.businessTagMasterTagCategoryLookupTableModal.show();
    }
    openSelectMasterTagModal() {
        this.businessTagMasterTagLookupTableModal.id = this.businessTag.masterTagId;
        this.businessTagMasterTagLookupTableModal.displayName = this.masterTagName;
        this.businessTagMasterTagLookupTableModal.show();
    }

    setBusinessIdNull() {
        this.businessTag.businessId = null;
        this.businessName = '';
    }
    setMasterTagCategoryIdNull() {
        this.businessTag.masterTagCategoryId = null;
        this.masterTagCategoryName = '';
    }
    setMasterTagIdNull() {
        this.businessTag.masterTagId = null;
        this.masterTagName = '';
    }

    getNewBusinessId() {
        this.businessTag.businessId = this.businessTagBusinessLookupTableModal.id;
        this.businessName = this.businessTagBusinessLookupTableModal.displayName;
    }
    getNewMasterTagCategoryId() {
        this.businessTag.masterTagCategoryId = this.businessTagMasterTagCategoryLookupTableModal.id;
        this.masterTagCategoryName = this.businessTagMasterTagCategoryLookupTableModal.displayName;
    }
    getNewMasterTagId() {
        this.businessTag.masterTagId = this.businessTagMasterTagLookupTableModal.id;
        this.masterTagName = this.businessTagMasterTagLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
