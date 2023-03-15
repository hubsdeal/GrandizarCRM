import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { SmsTemplatesServiceProxy, CreateOrEditSmsTemplateDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditSmsTemplateModal',
    templateUrl: './create-or-edit-smsTemplate-modal.component.html',
})
export class CreateOrEditSmsTemplateModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    smsTemplate: CreateOrEditSmsTemplateDto = new CreateOrEditSmsTemplateDto();

    constructor(
        injector: Injector,
        private _smsTemplatesServiceProxy: SmsTemplatesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(smsTemplateId?: number): void {
        if (!smsTemplateId) {
            this.smsTemplate = new CreateOrEditSmsTemplateDto();
            this.smsTemplate.id = smsTemplateId;

            this.active = true;
            this.modal.show();
        } else {
            this._smsTemplatesServiceProxy.getSmsTemplateForEdit(smsTemplateId).subscribe((result) => {
                this.smsTemplate = result.smsTemplate;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._smsTemplatesServiceProxy
            .createOrEdit(this.smsTemplate)
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
