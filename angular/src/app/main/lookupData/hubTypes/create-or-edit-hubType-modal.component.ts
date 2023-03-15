import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { HubTypesServiceProxy, CreateOrEditHubTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditHubTypeModal',
    templateUrl: './create-or-edit-hubType-modal.component.html',
})
export class CreateOrEditHubTypeModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    hubType: CreateOrEditHubTypeDto = new CreateOrEditHubTypeDto();

    constructor(
        injector: Injector,
        private _hubTypesServiceProxy: HubTypesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(hubTypeId?: number): void {
        if (!hubTypeId) {
            this.hubType = new CreateOrEditHubTypeDto();
            this.hubType.id = hubTypeId;

            this.active = true;
            this.modal.show();
        } else {
            this._hubTypesServiceProxy.getHubTypeForEdit(hubTypeId).subscribe((result) => {
                this.hubType = result.hubType;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._hubTypesServiceProxy
            .createOrEdit(this.hubType)
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
