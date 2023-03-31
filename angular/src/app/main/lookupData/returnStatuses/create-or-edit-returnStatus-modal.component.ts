import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ReturnStatusesServiceProxy, CreateOrEditReturnStatusDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditReturnStatusModal',
    templateUrl: './create-or-edit-returnStatus-modal.component.html',
})
export class CreateOrEditReturnStatusModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    returnStatus: CreateOrEditReturnStatusDto = new CreateOrEditReturnStatusDto();

    constructor(
        injector: Injector,
        private _returnStatusesServiceProxy: ReturnStatusesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(returnStatusId?: number): void {
        if (!returnStatusId) {
            this.returnStatus = new CreateOrEditReturnStatusDto();
            this.returnStatus.id = returnStatusId;

            this.active = true;
            this.modal.show();
        } else {
            this._returnStatusesServiceProxy.getReturnStatusForEdit(returnStatusId).subscribe((result) => {
                this.returnStatus = result.returnStatus;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._returnStatusesServiceProxy
            .createOrEdit(this.returnStatus)
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
