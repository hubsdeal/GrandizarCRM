import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { LeadSourcesServiceProxy, CreateOrEditLeadSourceDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditLeadSourceModal',
    templateUrl: './create-or-edit-leadSource-modal.component.html',
})
export class CreateOrEditLeadSourceModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    leadSource: CreateOrEditLeadSourceDto = new CreateOrEditLeadSourceDto();

    constructor(
        injector: Injector,
        private _leadSourcesServiceProxy: LeadSourcesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(leadSourceId?: number): void {
        if (!leadSourceId) {
            this.leadSource = new CreateOrEditLeadSourceDto();
            this.leadSource.id = leadSourceId;

            this.active = true;
            this.modal.show();
        } else {
            this._leadSourcesServiceProxy.getLeadSourceForEdit(leadSourceId).subscribe((result) => {
                this.leadSource = result.leadSource;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._leadSourcesServiceProxy
            .createOrEdit(this.leadSource)
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
