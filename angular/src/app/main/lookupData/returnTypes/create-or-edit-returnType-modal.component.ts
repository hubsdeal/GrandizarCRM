import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ReturnTypesServiceProxy, CreateOrEditReturnTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditReturnTypeModal',
    templateUrl: './create-or-edit-returnType-modal.component.html',
})
export class CreateOrEditReturnTypeModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    returnType: CreateOrEditReturnTypeDto = new CreateOrEditReturnTypeDto();

    constructor(
        injector: Injector,
        private _returnTypesServiceProxy: ReturnTypesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(returnTypeId?: number): void {
        if (!returnTypeId) {
            this.returnType = new CreateOrEditReturnTypeDto();
            this.returnType.id = returnTypeId;

            this.active = true;
            this.modal.show();
        } else {
            this._returnTypesServiceProxy.getReturnTypeForEdit(returnTypeId).subscribe((result) => {
                this.returnType = result.returnType;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._returnTypesServiceProxy
            .createOrEdit(this.returnType)
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
