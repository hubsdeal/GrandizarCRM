import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    OrderSalesChannelsServiceProxy,
    CreateOrEditOrderSalesChannelDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditOrderSalesChannelModal',
    templateUrl: './create-or-edit-orderSalesChannel-modal.component.html',
})
export class CreateOrEditOrderSalesChannelModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    orderSalesChannel: CreateOrEditOrderSalesChannelDto = new CreateOrEditOrderSalesChannelDto();

    constructor(
        injector: Injector,
        private _orderSalesChannelsServiceProxy: OrderSalesChannelsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(orderSalesChannelId?: number): void {
        if (!orderSalesChannelId) {
            this.orderSalesChannel = new CreateOrEditOrderSalesChannelDto();
            this.orderSalesChannel.id = orderSalesChannelId;

            this.active = true;
            this.modal.show();
        } else {
            this._orderSalesChannelsServiceProxy
                .getOrderSalesChannelForEdit(orderSalesChannelId)
                .subscribe((result) => {
                    this.orderSalesChannel = result.orderSalesChannel;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._orderSalesChannelsServiceProxy
            .createOrEdit(this.orderSalesChannel)
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
