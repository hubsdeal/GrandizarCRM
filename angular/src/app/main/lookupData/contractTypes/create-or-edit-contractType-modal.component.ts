import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ContractTypesServiceProxy, CreateOrEditContractTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditContractTypeModal',
    templateUrl: './create-or-edit-contractType-modal.component.html',
})
export class CreateOrEditContractTypeModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    contractType: CreateOrEditContractTypeDto = new CreateOrEditContractTypeDto();

    constructor(
        injector: Injector,
        private _contractTypesServiceProxy: ContractTypesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(contractTypeId?: number): void {
        if (!contractTypeId) {
            this.contractType = new CreateOrEditContractTypeDto();
            this.contractType.id = contractTypeId;

            this.active = true;
            this.modal.show();
        } else {
            this._contractTypesServiceProxy.getContractTypeForEdit(contractTypeId).subscribe((result) => {
                this.contractType = result.contractType;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._contractTypesServiceProxy
            .createOrEdit(this.contractType)
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
