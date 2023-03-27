import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    LeadPipelineStagesServiceProxy,
    CreateOrEditLeadPipelineStageDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditLeadPipelineStageModal',
    templateUrl: './create-or-edit-leadPipelineStage-modal.component.html',
})
export class CreateOrEditLeadPipelineStageModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    leadPipelineStage: CreateOrEditLeadPipelineStageDto = new CreateOrEditLeadPipelineStageDto();

    constructor(
        injector: Injector,
        private _leadPipelineStagesServiceProxy: LeadPipelineStagesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(leadPipelineStageId?: number): void {
        if (!leadPipelineStageId) {
            this.leadPipelineStage = new CreateOrEditLeadPipelineStageDto();
            this.leadPipelineStage.id = leadPipelineStageId;

            this.active = true;
            this.modal.show();
        } else {
            this._leadPipelineStagesServiceProxy
                .getLeadPipelineStageForEdit(leadPipelineStageId)
                .subscribe((result) => {
                    this.leadPipelineStage = result.leadPipelineStage;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._leadPipelineStagesServiceProxy
            .createOrEdit(this.leadPipelineStage)
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
