import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { BusinessTaskMapsServiceProxy, CreateOrEditBusinessTaskMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { BusinessTaskMapBusinessLookupTableModalComponent } from './businessTaskMap-business-lookup-table-modal.component';
import { BusinessTaskMapTaskEventLookupTableModalComponent } from './businessTaskMap-taskEvent-lookup-table-modal.component';

@Component({
    selector: 'createOrEditBusinessTaskMapModal',
    templateUrl: './create-or-edit-businessTaskMap-modal.component.html',
})
export class CreateOrEditBusinessTaskMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('businessTaskMapBusinessLookupTableModal', { static: true })
    businessTaskMapBusinessLookupTableModal: BusinessTaskMapBusinessLookupTableModalComponent;
    @ViewChild('businessTaskMapTaskEventLookupTableModal', { static: true })
    businessTaskMapTaskEventLookupTableModal: BusinessTaskMapTaskEventLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    businessTaskMap: CreateOrEditBusinessTaskMapDto = new CreateOrEditBusinessTaskMapDto();

    businessName = '';
    taskEventName = '';

    constructor(
        injector: Injector,
        private _businessTaskMapsServiceProxy: BusinessTaskMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(businessTaskMapId?: number): void {
        if (!businessTaskMapId) {
            this.businessTaskMap = new CreateOrEditBusinessTaskMapDto();
            this.businessTaskMap.id = businessTaskMapId;
            this.businessName = '';
            this.taskEventName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._businessTaskMapsServiceProxy.getBusinessTaskMapForEdit(businessTaskMapId).subscribe((result) => {
                this.businessTaskMap = result.businessTaskMap;

                this.businessName = result.businessName;
                this.taskEventName = result.taskEventName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._businessTaskMapsServiceProxy
            .createOrEdit(this.businessTaskMap)
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

    openSelectBusinessModal() {
        this.businessTaskMapBusinessLookupTableModal.id = this.businessTaskMap.businessId;
        this.businessTaskMapBusinessLookupTableModal.displayName = this.businessName;
        this.businessTaskMapBusinessLookupTableModal.show();
    }
    openSelectTaskEventModal() {
        this.businessTaskMapTaskEventLookupTableModal.id = this.businessTaskMap.taskEventId;
        this.businessTaskMapTaskEventLookupTableModal.displayName = this.taskEventName;
        this.businessTaskMapTaskEventLookupTableModal.show();
    }

    setBusinessIdNull() {
        this.businessTaskMap.businessId = null;
        this.businessName = '';
    }
    setTaskEventIdNull() {
        this.businessTaskMap.taskEventId = null;
        this.taskEventName = '';
    }

    getNewBusinessId() {
        this.businessTaskMap.businessId = this.businessTaskMapBusinessLookupTableModal.id;
        this.businessName = this.businessTaskMapBusinessLookupTableModal.displayName;
    }
    getNewTaskEventId() {
        this.businessTaskMap.taskEventId = this.businessTaskMapTaskEventLookupTableModal.id;
        this.taskEventName = this.businessTaskMapTaskEventLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
