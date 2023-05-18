import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    BusinessDocumentsServiceProxy,
    CreateOrEditBusinessDocumentDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { BusinessDocumentBusinessLookupTableModalComponent } from './businessDocument-business-lookup-table-modal.component';
import { BusinessDocumentDocumentTypeLookupTableModalComponent } from './businessDocument-documentType-lookup-table-modal.component';

@Component({
    selector: 'createOrEditBusinessDocumentModal',
    templateUrl: './create-or-edit-businessDocument-modal.component.html',
})
export class CreateOrEditBusinessDocumentModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('businessDocumentBusinessLookupTableModal', { static: true })
    businessDocumentBusinessLookupTableModal: BusinessDocumentBusinessLookupTableModalComponent;
    @ViewChild('businessDocumentDocumentTypeLookupTableModal', { static: true })
    businessDocumentDocumentTypeLookupTableModal: BusinessDocumentDocumentTypeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    businessDocument: CreateOrEditBusinessDocumentDto = new CreateOrEditBusinessDocumentDto();

    businessName = '';
    documentTypeName = '';

    constructor(
        injector: Injector,
        private _businessDocumentsServiceProxy: BusinessDocumentsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(businessDocumentId?: number): void {
        if (!businessDocumentId) {
            this.businessDocument = new CreateOrEditBusinessDocumentDto();
            this.businessDocument.id = businessDocumentId;
            this.businessName = '';
            this.documentTypeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._businessDocumentsServiceProxy.getBusinessDocumentForEdit(businessDocumentId).subscribe((result) => {
                this.businessDocument = result.businessDocument;

                this.businessName = result.businessName;
                this.documentTypeName = result.documentTypeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._businessDocumentsServiceProxy
            .createOrEdit(this.businessDocument)
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
        this.businessDocumentBusinessLookupTableModal.id = this.businessDocument.businessId;
        this.businessDocumentBusinessLookupTableModal.displayName = this.businessName;
        this.businessDocumentBusinessLookupTableModal.show();
    }
    openSelectDocumentTypeModal() {
        this.businessDocumentDocumentTypeLookupTableModal.id = this.businessDocument.documentTypeId;
        this.businessDocumentDocumentTypeLookupTableModal.displayName = this.documentTypeName;
        this.businessDocumentDocumentTypeLookupTableModal.show();
    }

    setBusinessIdNull() {
        this.businessDocument.businessId = null;
        this.businessName = '';
    }
    setDocumentTypeIdNull() {
        this.businessDocument.documentTypeId = null;
        this.documentTypeName = '';
    }

    getNewBusinessId() {
        this.businessDocument.businessId = this.businessDocumentBusinessLookupTableModal.id;
        this.businessName = this.businessDocumentBusinessLookupTableModal.displayName;
    }
    getNewDocumentTypeId() {
        this.businessDocument.documentTypeId = this.businessDocumentDocumentTypeLookupTableModal.id;
        this.documentTypeName = this.businessDocumentDocumentTypeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
