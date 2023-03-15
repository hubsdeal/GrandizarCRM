import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { EmailTemplatesServiceProxy, CreateOrEditEmailTemplateDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditEmailTemplateModal',
    templateUrl: './create-or-edit-emailTemplate-modal.component.html',
})
export class CreateOrEditEmailTemplateModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    emailTemplate: CreateOrEditEmailTemplateDto = new CreateOrEditEmailTemplateDto();

    constructor(
        injector: Injector,
        private _emailTemplatesServiceProxy: EmailTemplatesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(emailTemplateId?: number): void {
        if (!emailTemplateId) {
            this.emailTemplate = new CreateOrEditEmailTemplateDto();
            this.emailTemplate.id = emailTemplateId;

            this.active = true;
            this.modal.show();
        } else {
            this._emailTemplatesServiceProxy.getEmailTemplateForEdit(emailTemplateId).subscribe((result) => {
                this.emailTemplate = result.emailTemplate;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._emailTemplatesServiceProxy
            .createOrEdit(this.emailTemplate)
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
