import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { DocumentTypesServiceProxy, CreateOrEditDocumentTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditDocumentTypeModal',
    templateUrl: './create-or-edit-documentType-modal.component.html',
})
export class CreateOrEditDocumentTypeModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    documentType: CreateOrEditDocumentTypeDto = new CreateOrEditDocumentTypeDto();

    constructor(
        injector: Injector,
        private _documentTypesServiceProxy: DocumentTypesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(documentTypeId?: number): void {
        if (!documentTypeId) {
            this.documentType = new CreateOrEditDocumentTypeDto();
            this.documentType.id = documentTypeId;

            this.active = true;
            this.modal.show();
        } else {
            this._documentTypesServiceProxy.getDocumentTypeForEdit(documentTypeId).subscribe((result) => {
                this.documentType = result.documentType;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._documentTypesServiceProxy
            .createOrEdit(this.documentType)
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
