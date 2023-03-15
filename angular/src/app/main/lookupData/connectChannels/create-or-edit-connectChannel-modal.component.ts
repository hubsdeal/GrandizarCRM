import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ConnectChannelsServiceProxy, CreateOrEditConnectChannelDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditConnectChannelModal',
    templateUrl: './create-or-edit-connectChannel-modal.component.html',
})
export class CreateOrEditConnectChannelModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    connectChannel: CreateOrEditConnectChannelDto = new CreateOrEditConnectChannelDto();

    constructor(
        injector: Injector,
        private _connectChannelsServiceProxy: ConnectChannelsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(connectChannelId?: number): void {
        if (!connectChannelId) {
            this.connectChannel = new CreateOrEditConnectChannelDto();
            this.connectChannel.id = connectChannelId;

            this.active = true;
            this.modal.show();
        } else {
            this._connectChannelsServiceProxy.getConnectChannelForEdit(connectChannelId).subscribe((result) => {
                this.connectChannel = result.connectChannel;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._connectChannelsServiceProxy
            .createOrEdit(this.connectChannel)
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
