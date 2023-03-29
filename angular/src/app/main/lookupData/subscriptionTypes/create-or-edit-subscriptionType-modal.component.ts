import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    SubscriptionTypesServiceProxy,
    CreateOrEditSubscriptionTypeDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditSubscriptionTypeModal',
    templateUrl: './create-or-edit-subscriptionType-modal.component.html',
})
export class CreateOrEditSubscriptionTypeModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    subscriptionType: CreateOrEditSubscriptionTypeDto = new CreateOrEditSubscriptionTypeDto();

    constructor(
        injector: Injector,
        private _subscriptionTypesServiceProxy: SubscriptionTypesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(subscriptionTypeId?: number): void {
        if (!subscriptionTypeId) {
            this.subscriptionType = new CreateOrEditSubscriptionTypeDto();
            this.subscriptionType.id = subscriptionTypeId;

            this.active = true;
            this.modal.show();
        } else {
            this._subscriptionTypesServiceProxy.getSubscriptionTypeForEdit(subscriptionTypeId).subscribe((result) => {
                this.subscriptionType = result.subscriptionType;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._subscriptionTypesServiceProxy
            .createOrEdit(this.subscriptionType)
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
