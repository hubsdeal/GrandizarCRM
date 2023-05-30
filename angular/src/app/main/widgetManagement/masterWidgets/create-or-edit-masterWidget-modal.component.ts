import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { MasterWidgetsServiceProxy, CreateOrEditMasterWidgetDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditMasterWidgetModal',
    templateUrl: './create-or-edit-masterWidget-modal.component.html',
})
export class CreateOrEditMasterWidgetModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    masterWidget: CreateOrEditMasterWidgetDto = new CreateOrEditMasterWidgetDto();

    constructor(
        injector: Injector,
        private _masterWidgetsServiceProxy: MasterWidgetsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(masterWidgetId?: number): void {
        if (!masterWidgetId) {
            this.masterWidget = new CreateOrEditMasterWidgetDto();
            this.masterWidget.id = masterWidgetId;

            this.active = true;
            this.modal.show();
        } else {
            this._masterWidgetsServiceProxy.getMasterWidgetForEdit(masterWidgetId).subscribe((result) => {
                this.masterWidget = result.masterWidget;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._masterWidgetsServiceProxy
            .createOrEdit(this.masterWidget)
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
